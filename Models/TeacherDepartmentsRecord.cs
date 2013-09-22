using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ambition.School.Core.Models
{
    public class TeacherDepartmentsRecord {
        public virtual int Id { get; set; }
        public virtual TeacherPartRecord TeacherPartRecord { get; set; }
        public virtual PositionsDepartmentRecord PositionsDepartmentRecord { get; set; }
    }
}