using System;
using System.ComponentModel.DataAnnotations;

namespace ProductCategories.Models
{
    public class ProductViewModel
    {
        [Required]
        public string Name{get;set;}
        [Required]
        public string description{get;set;}
        [Required]
        public double price{get;set;}
    }
}