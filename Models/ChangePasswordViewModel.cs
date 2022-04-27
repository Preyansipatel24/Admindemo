using System.ComponentModel.DataAnnotations;

namespace AdminDemo.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmNewPassword")]
        [Compare("NewPassword",ErrorMessage = "The new password and confirmation password do not match. ")]
        public string ConfirmPassword { get; set; }
    }
}
