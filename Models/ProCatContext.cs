using Microsoft.EntityFrameworkCore;

namespace ProductCategories.Models{
    public class ProCatContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public ProCatContext(DbContextOptions<ProCatContext> options) : base(options) { }

        public DbSet<users> users{get;set;}

        public DbSet<categories> categories{get;set;}

        public DbSet<products> products{get;set;}

        public DbSet<productscategories> productscategories{get;set;}
        
    }
}