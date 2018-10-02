using System;
using System.ComponentModel.DataAnnotations;

namespace ProductCategories.Models
{
    public class CategoryViewModel
    {
        [Required]
        public string Name{get;set;}
    }
}