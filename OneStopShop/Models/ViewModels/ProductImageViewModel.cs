using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models.ViewModels
{
	public class ProductImageViewModel
	{
        [Required(ErrorMessage = "Please enter a product name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Please enter a positive price")]
        public decimal ProductPrice { get; set; }

        [Required(ErrorMessage = "Please enter a Created Date")]
        public DateTime ProductCreatedDate { get; set; }
        public DateTime ProductModifiedDate { get; set; }

        [Required(ErrorMessage = "Please choose product image")]
        public IFormFile ProductImage { get; set; }
        public string ProductSize { get; set; }
        public string ProductColor { get; set; }
    }
}
