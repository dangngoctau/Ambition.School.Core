using Orchard.Data.Conventions;

namespace Ambition.School.Core.Models
{
    public class PositionsDepartmentRecord
    {
        public virtual int Id { get; set; }
        [Aggregate]
        public virtual PositionRecord PositionRecord { get; set; }
        public virtual DepartmentRecord DepartmentRecord { get; set; }
    }
}