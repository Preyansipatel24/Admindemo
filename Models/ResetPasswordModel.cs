namespace AdminDemo.Models
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string code { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
       
    }
}
