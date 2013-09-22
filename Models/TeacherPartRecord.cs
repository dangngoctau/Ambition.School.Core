using Orchard.ContentManagement.Records;

namespace Ambition.School.Core.Models
{
    public class TeacherPartRecord : ContentPartRecord
    {
        public virtual string PositionsDepartments { get; set; }
    }
}