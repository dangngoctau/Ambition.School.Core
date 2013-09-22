using System.ComponentModel.DataAnnotations;
using Ambition.School.Core.Extensions;

namespace Ambition.School.Core.ViewModels
{
    public class UserCreateViewModel
    {
        [Required, DataType(DataType.EmailAddress), LocalizedDisplayName("Email", Scope)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), LocalizedDisplayName("Password", Scope)]
        [StringLength(50, MinimumLength = 7)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), LocalizedDisplayName("ConfirmPassword", Scope)]
        public string ConfirmPassword { get; set; }

        private const string Scope = Constants.LocalArea + ".ViewModels.UserCreateViewModel";
    }
}