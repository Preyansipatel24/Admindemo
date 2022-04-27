using System.ComponentModel.DataAnnotations;

namespace AdminDemo.Models
{
    public class RollMaster
    {
        [Key]
        public int RollId { get; set; }
        [Required(ErrorMessage = "Enter Your RollName")]
        public string RollName { get; set; }
    }
}
