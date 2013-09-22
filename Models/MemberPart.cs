using System;
using System.ComponentModel.DataAnnotations;
using Ambition.School.Core.Extensions;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;

namespace Ambition.School.Core.Models
{
    public class MemberPart : ContentPart<MemberPartRecord>, ITitleAspect
    {
        [Required, LocalizedDisplayName("FullName", Scope)]
        public string FullName
        {
            get { return Record.FullName; }
            set { Record.FullName = value; }
        }

        [LocalizedDisplayName("FirstName", Scope)]
        public string FirstName
        {
            get { return Record.FirstName; }
            set { Record.FirstName = value; }
        }

        [LocalizedDisplayName("LastName", Scope)]
        public string LastName
        {
            get { return Record.LastName; }
            set { Record.LastName = value; }
        }

        [LocalizedDisplayName("Birthday", Scope)]
        public DateTime? Birthday
        {
            get { return Record.Birthday; }
            set { Record.Birthday = value; }
        }

        [LocalizedDisplayName("Address", Scope)]
        public string Address
        {
            get { return Record.Address; }
            set { Record.Address = value; }
        }

        public int UserId
        {
            get { return Record.UserId; }
        }

        public string Title
        {
            get { return Record.FullName; }
        }

        private const string Scope = Constants.LocalArea + ".Models.MemberPart";
    }
}