using System.ComponentModel.DataAnnotations;
using Ambition.School.Core.Extensions;
using Orchard.ContentManagement;

namespace Ambition.School.Core.Models
{
    public class TeacherPart : ContentPart<TeacherPartRecord>
    {
        [Required, LocalizedDisplayName("PositionsDepartments", Scope)]
        public string PositionsDepartments
        {
            get { return Record.PositionsDepartments; }
            set { Record.PositionsDepartments = value; }
        }

        private const string Scope = Constants.LocalArea + ".Models.TeacherPart";
    }
}