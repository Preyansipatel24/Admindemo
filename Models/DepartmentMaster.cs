using System.ComponentModel.DataAnnotations;

namespace AdminDemo.Models
{
    public class DepartmentMaster
    {
        [Key]
        public int DeptId { get; set; }
        [Required(ErrorMessage ="Enter your DepartmentName")]
        public string DepartmentName { get; set; }
    }
}
