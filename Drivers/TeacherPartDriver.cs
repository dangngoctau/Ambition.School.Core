using System;
using System.Linq;
using Ambition.School.Core.Models;
using Ambition.School.Core.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;
using Orchard.Users.Services;

namespace Ambition.School.Core.Drivers
{
    public class TeacherPartDriver : ContentPartDriver<TeacherPart>
    {
        private readonly IMembershipService _membershipService;
        private readonly IUserService _userService;
        private readonly IOrchardServices _orchardServices;
        private readonly IDepartmentService _departmentService;
        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public TeacherPartDriver(IMembershipService membershipService, IUserService userService, IOrchardServices orchardServices, IDepartmentService departmentService)
        {
            _membershipService = membershipService;
            _userService = userService;
            _orchardServices = orchardServices;
            _departmentService = departmentService;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        protected override DriverResult Editor(TeacherPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Teacher_Edit",
                () => shapeHelper.EditorTemplate(TemplateName: "Parts.Teacher", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(TeacherPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
                if (string.IsNullOrWhiteSpace(part.PositionsDepartments))
                {
                    updater.AddModelError(Prefix, T("Please choose positions which this teacher belongs to."));
                }
                else
                {
                    try
                    {
                        var positionsSplited = part.PositionsDepartments.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
                        if (_departmentService.CheckPositionsDepartment(positionsSplited))
                        {
                            part.PositionsDepartments = string.Join(",", positionsSplited);
                        }
                        else
                        {
                            updater.AddModelError(Prefix, T("Tempar data."));
                        }
                    }
                    catch
                    {
                        updater.AddModelError(Prefix, T("Invalid contents in string."));
                    }
                }
            }
            return Editor(part, shapeHelper);
        }
    }
}