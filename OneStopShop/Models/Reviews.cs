using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models
{
    public class Reviews
    {
        [Key]
        public int ReviewID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product product { get; set; }

        [ForeignKey("UserID")]
        public virtual Users user { get; set; }

        [Required(ErrorMessage = "Please enter a rating"), Range(1, 5, ErrorMessage = "Rating must be between 1 star and 5 star")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Please enter a review description")]
        public string ReviewDescription { get; set; }
    }
}
