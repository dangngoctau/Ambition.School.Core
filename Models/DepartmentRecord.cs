using System.Collections.Generic;
using Orchard.Data.Conventions;

namespace Ambition.School.Core.Models
{
    public class DepartmentRecord
    {
        public DepartmentRecord()
        {
            PositionDepartmentRecords = new List<PositionsDepartmentRecord>();
        }
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DepartmentRecord ParentDepartmentRecord { get; set; }
        [CascadeAllDeleteOrphan, Aggregate]
        public virtual IList<PositionsDepartmentRecord> PositionDepartmentRecords { get; set; }
    }
}