using Ambition.School.Core.Models;

namespace Ambition.School.Core.ViewModels
{
    public class MemberCreateViewModel
    {
        public UserCreateViewModel User { get; set; }
        public MemberPart Member { get; set; }
    }
}