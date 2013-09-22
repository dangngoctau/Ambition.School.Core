using System;
using System.Linq;
using Ambition.School.Core.Models;
using Ambition.School.Core.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;

namespace Ambition.School.Core.Drivers
{
    public class ArticlePermissionPartDriver : ContentPartDriver<ArticlePermissionPart>
    {
        public Localizer T { get; set; }
        private readonly IDepartmentService _departmentService;
        public ArticlePermissionPartDriver(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
            T = NullLocalizer.Instance;
        }

        protected override DriverResult Editor(ArticlePermissionPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_ArticlePermission_Edit",
                () => shapeHelper.EditorTemplate(TemplateName: "Parts.ArticlePermission", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(ArticlePermissionPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            if (part.IsEnabled)
            {
                if (string.IsNullOrWhiteSpace(part.Permissions))
                {
                    updater.AddModelError(Prefix, T("Please choose positions which have right to view this content."));
                }
                else
                {
                    try
                    {
                        var permissionSplited = part.Permissions.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
                        if (_departmentService.CheckPositionsDepartment(permissionSplited))
                        {
                            part.Permissions = string.Join(",", permissionSplited);
                        }
                        else
                        {
                            updater.AddModelError(Prefix, T("Tempar data."));
                        }
                    }
                    catch
                    {
                        updater.AddModelError(Prefix, T("Invalid contents in permissions string."));
                    }
                }
            }
            else
            {
                part.Permissions = string.Empty;
            }
            return Editor(part, shapeHelper);
        }

        protected override string Prefix
        {
            get { return "ArticlePermission"; }
        }
    }
}