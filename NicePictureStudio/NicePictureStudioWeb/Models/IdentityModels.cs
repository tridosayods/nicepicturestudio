using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;

namespace NicePictureStudio.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name="Full Name")]
        public string Name { get; set; }

        public string Position { get; set; }
        
        [Display(Name="Supervisor Name (If any)")]
        public int ManagerId { get; set; }
        
        [Display(Name="Start Date of Working")]
        public DateTime StartDate { get; set; }

        public int Status { get; set; }

        [Display(Name="Identification Number")]
        public int IdentificationNumber { get; set; }

        [Display(Name = "Mobile Phone Number")]
        public override string PhoneNumber { get; set; }

        public string Address { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Education { get; set; }
        public string Specialability { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<IdentityUser>().ToTable("Employee").Property(p => p.Id).HasColumnName("EmployeeId");
            
            modelBuilder.Entity<ApplicationUser>().ToTable("Employee").Property(p => p.Id).HasColumnName("EmployeeId");
            modelBuilder.Entity<IdentityUserRole>().ToTable("EmployeeRole");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("EmployeeLogin");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("EmployeeClaim");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            //modelBuilder.Entity<ApplicationRole>().ToTable("Role");
            this.Configuration.LazyLoadingEnabled = true;
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            //Disable option for migration database
            //Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<NicePictureStudio.App_Data.Service> Services { get; set; }

        public System.Data.Entity.DbSet<NicePictureStudio.App_Data.Customer> Customers { get; set; }
    }
}