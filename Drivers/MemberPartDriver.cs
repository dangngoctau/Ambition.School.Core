using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Ambition.School.Core.Extensions;
using Ambition.School.Core.Models;
using Ambition.School.Core.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;
using Orchard.Users.Services;
using Orchard.Users.Models;

namespace Ambition.School.Core.Drivers
{
    public class MemberPartDriver : ContentPartDriver<MemberPart>
    {
        private readonly IMembershipService _membershipService;
        private readonly IUserService _userService;
        private readonly IOrchardServices _orchardServices;
        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public MemberPartDriver(IMembershipService membershipService, IUserService userService, IOrchardServices orchardServices)
        {
            _membershipService = membershipService;
            _userService = userService;
            _orchardServices = orchardServices;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        private IUser GetUser(int id)
        {
            return _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.Id == id).List().FirstOrDefault();
        }

        protected override string Prefix
        {
            get { return "Member"; }
        }

        private string GetPrefix(string name)
        {
            return string.Format("{0}.{1}", Prefix, name);
        }
        protected override DriverResult Editor(MemberPart part, dynamic shapeHelper)
        {
            var results = new List<DriverResult> {
                ContentShape("Parts_Member_Edit",
                    () => shapeHelper.EditorTemplate(TemplateName: "Parts.Member", Model: part, Prefix: GetPrefix("Member")))
            };
            if (_orchardServices.WorkContext.HttpContext.Request.RequestContext.RouteData.CheckAction(Actions.Create))
            {
                results.Add(ContentShape("Parts_Member_User_Create",
                    () => shapeHelper.EditorTemplate(TemplateName: "Parts.User.Create", Model: new UserCreateViewModel(), Prefix: GetPrefix("User"))));
            }
            else
            {
                //todo: is it necessary to update user info here?
                var editUser = new UserEditViewModel { Email = GetUser(part.UserId).Email };
                results.Add(ContentShape("Parts_Member_User_Create",
                    () => shapeHelper.EditorTemplate(TemplateName: "Parts.User.Edit", Model: editUser, Prefix: GetPrefix("User"))));

            }
            return Combined(results.ToArray());
        }

        protected override DriverResult Editor(MemberPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var controller = (Controller)updater;
            if (controller.RouteData.CheckAction(Actions.Create))
            {
                //Step 1. Validate all inputs.
                var createModel = new MemberCreateViewModel { Member = part };
                if (updater.TryUpdateModel(createModel, Prefix, null, null))
                {
                    //Step 2. Create user.
                    if (!Regex.IsMatch(createModel.User.Email ?? "", UserPart.EmailPattern, RegexOptions.IgnoreCase))
                    {
                        // http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx    
                        updater.AddModelError("Email", T("You must specify a valid email address."));
                    }
                    else if (createModel.User.Password != createModel.User.ConfirmPassword)
                    {
                        updater.AddModelError("ConfirmPassword", T("Password confirmation must match."));
                    }
                    else if (!_userService.VerifyUserUnicity(createModel.User.Email, createModel.User.Email))
                    {
                        updater.AddModelError("NotUniqueUserName", T("User with that email already exists."));
                    }
                    else if (controller.ModelState.IsValid)
                    {
                        IUser user = _membershipService.CreateUser(new CreateUserParams(createModel.User.Email, createModel.User.Password, createModel.User.Email, null, null, true));
                        //Step 2. Update userid on member.
                        part.Record.UserId = user.Id;
                    }
                }
            }
            else
            {
                var editModel = new MemberEditViewModel { Member = part };
                //todo: update email.
                updater.TryUpdateModel(editModel, Prefix, null, null);
            }
            return Editor(part, shapeHelper);
        }
    }
}