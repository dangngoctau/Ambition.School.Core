using System.Collections.Generic;
using Ambition.School.Core.Models;

namespace Ambition.School.Core.ViewModels
{
    public class ArticlePermissionCreateViewModel
    {
        public ArticlePermissionPart ArticlePermission { get; set; }
        public IList<DepartmentRecord> AvailableDepartments { get; set; }
    }
}