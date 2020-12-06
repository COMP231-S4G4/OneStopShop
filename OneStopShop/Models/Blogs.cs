using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneStopShop.Models
{
    public class Blogs
    {
        [Key]
        public int BlogId { get; set; }

        [DisplayName("Store Name")]
        public int StoreId { get; set; }

        public string BlogImage { get; set; }

        [Required(ErrorMessage = "Please enter a Blog Title")]
        public string BlogTitle { get; set; }

        public DateTime BlogCreatedDate { get; set; }

        public DateTime BlogModifiedDate { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        public string BlogDescription { get; set; }

        [DisplayName("Upload File")]
        [NotMapped]
        public IFormFile BlogFile { get; set; }
    }
}