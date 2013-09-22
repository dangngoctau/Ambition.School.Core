using System.ComponentModel.DataAnnotations;

namespace Ambition.School.Core.ViewModels {
    public class UserEditViewModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}