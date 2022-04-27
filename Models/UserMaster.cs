using Castle.MicroKernel.SubSystems.Conversion;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDemo.Models
{
    public class UserMaster
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Enter your FullName")]
        [Column(TypeName = "VARCHAR(250)")]
        public string FullName { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Enter your RollName")]
        public int RollId { get; set; }
       
        
        [Required(ErrorMessage = "Enter your DepartmentName")]
        public int DeptId { get; set; }

       // [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Enter your UserName")]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Enter your Password")]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        //public string ConfirmPassword { get; set; }
        public bool isactive { get; set; }
        public bool isDelete { get; set; }
        [Column(TypeName = "VARCHAR(250)")]
        public string CreatedBy { get; set; }
        [Column(TypeName = "VARCHAR(250)")]
        public string UpdatedBy { get; set; }
        public DateTime CreateOn { get; set; } = DateTime.Now;

        public DateTime UpdateOn { get; set; }
        [Column(TypeName = "VARCHAR(500)")]
        public string ResetPasswordCode { get;  set; }
    }
    }



