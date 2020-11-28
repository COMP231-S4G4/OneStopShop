using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please enter your Username")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [MinLength(5, ErrorMessage = "The Address must be at least 5 characters long")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNum { get; set; }

        [Required(ErrorMessage = "Please enter your Account Type")]
        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Bank Account#")]
        public int AccountNumber { get; set; }

        [Display(Name = "Transit#")]
        public string TransitNumber { get; set; }

        [Display(Name = "Institution#")]
        public string InstitutionNumber { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
