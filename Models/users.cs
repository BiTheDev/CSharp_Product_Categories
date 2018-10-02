using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductCategories.Models
{
    public class users{

        public users(){
            UserProduct = new List<products>();
            UserCategories = new List<categories>();
        }
        
        [Key]
        public int id{get;set;}
        

        [Required]
        public string first_name{get; set;}

        [Required]
        public string last_name{get; set;}


        [Required]
        public string email {get; set;}

        [Required]
        public string password {get; set;}

        public DateTime created_at{get;set;}
        public DateTime updated_at{get; set;}

        public List<products> UserProduct{get;set;}

        public List<categories> UserCategories{get;set;}


    }
}