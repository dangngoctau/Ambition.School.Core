using System;
using Orchard.ContentManagement.Records;

namespace Ambition.School.Core.Models
{
    public class MemberPartRecord : ContentPartRecord
    {

        public virtual string FullName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime? Birthday { get; set; }
        public virtual string Address { get; set; }
        public virtual int UserId { get; set; }

    }
}