using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ambition.School.Core.Extensions;
using Ambition.School.Core.Models;

namespace Ambition.School.Core.ViewModels
{
    [Bind(Exclude = "AvailableDepartments")]
    public class DepartmentCreateViewModel
    {
        public DepartmentCreateViewModel()
        {
            AvailablePositions = new List<PositionViewModel>();
            AvailableDepartments = new List<DepartmentRecord>();
        }
        [Required, LocalizedDisplayName("Name", Scope)]
        public string Name { get; set; }
        public IList<PositionViewModel> AvailablePositions { get; set; }
        public IList<DepartmentRecord> AvailableDepartments { get; set; }
        [LocalizedDisplayName("Parent Department", Scope)]
        public int? ParentDepartmentId { get; set; }

        private const string Scope = Constants.LocalArea + ".ViewModels.DepartmentCreateViewModel";
    }
}