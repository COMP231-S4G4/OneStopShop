using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models
{
    public class CustomOrders
    {
        [Key]
        public int CustomOrderID { get; set; }

        [Required(ErrorMessage = "Please enter your Username")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [MinLength(5, ErrorMessage = "The Address must be at least 5 characters long")]
        [Display(Name = "Address")]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNum { get; set; }

        [Required(ErrorMessage = "Please enter a Created Date")]
        public DateTime OrderCreatedDate { get; set; }

        

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Type")]
        public string ProductType { get; set; }

        [Display(Name = "Chest Measurement")]
        public string Chest { get; set; }

        [Display(Name = "Neck Measurement")]
        public string Neck { get; set; }

        [Display(Name = "Shoulder Measurement")]
        public string Shoulder { get; set; }

        [Display(Name = "Sleeve Measurement")]
        public string Sleeve { get; set; }

        [Display(Name = "Waist Measurement")]
        public string Waist { get; set; }

        [Display(Name = "Hip Measurement")]
        public string Hip { get; set; }

        [Display(Name = "Inseam Measurement")]
        public string InseamLength { get; set; }

        [Display(Name = "Full Measurement")]
        public string FullLength { get; set; }

        [Display(Name = "Ankle Measurement")]
        public string AnkleLength { get; set; }
    }
}
