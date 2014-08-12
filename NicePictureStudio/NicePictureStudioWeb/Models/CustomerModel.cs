using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace NicePictureStudio.Models
{
    public class Customerxx
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime AnniversaryDate { get; set; }
    }

    public class CustomerDBContext : DbContext
    {
        public DbSet<Customerxx> Customers { get; set; }
        public CustomerDBContext() : base("DefaultConnection"){}
        protected override void OnModelCreating(DbModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customerxx>().HasKey(x => x.CustomerId);   
        }
            
    }
}