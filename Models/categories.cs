using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductCategories.Models
{
    public class categories{
        public categories(){
           CatPro = new  List<productscategories>();
        }
    

    [Key]
    public int id {get;set;}
    public int usersid{get;set;}

    public string name{get;set;}
    public users user{get;set;}
    public List<productscategories> CatPro{get;set;}
    public DateTime created_at{get;set;}
    public DateTime updated_at{get;set;}
    }
}