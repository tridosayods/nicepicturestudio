using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Web.Hosting;

namespace NicePictureStudio.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<string, ApplicationUserLogin,ApplicationUserRole,ApplicationUserClaim>
    //public class ApplicationUser : IdentityUser
    {
        //public ApplicationUser() : base() { }

        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public override string Id { get; set; }
        
        [Display(Name="Full Name")]
        public string Name { get; set; }

        public string Position { get; set; }
        
        [Display(Name="Supervisor Name (If any)")]
        public string ManagerId { get; set; }
        
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

        //public virtual NicePictureStudio.App_Data.EmployeeSchedule EmployeeSchedules { get; set; }
        //public virtual ICollection<NicePictureStudio.App_Data.EmployeeSchedule> EmployeeSchedules { get; set; }
       // public virtual NicePictureStudio.App_Data.EmployeeStatu EmployeeStatus { get; set; }
        //public virtual ICollection<NicePictureStudio.App_Data.EmployeeStatus> EmployeeStatuses { get; set; }
       // public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

            public class ApplicationUserLogin : IdentityUserLogin<string>
            {
            }

        public class ApplicationUserClaim : IdentityUserClaim<string>
        {
        }

        public class ApplicationUserRole : IdentityUserRole<string>
        {
        }

        public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
        //public class ApplicationRole : IdentityRole
        {
            public ApplicationRole() : base() { }
            //public ApplicationRole(string name, string description)
            //    : base()
            //{
            //    this.Description = Description;
            //}
            //public virtual string Description { get; set; }
            public ApplicationRole(string name, string description) { this.Name = name; this.Description = description; this.Id = Guid.NewGuid().ToString(); }
            public string Description { get; set; }
        }

        public class ApplicationRoleStore : RoleStore<ApplicationRole, string, ApplicationUserRole>
        {
            public ApplicationRoleStore(ApplicationDbContext context)
                : base(context)
            {
            }
        }

    public class ApplicationRoleManager : RoleManager<ApplicationRole,string>
    //public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
    public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore)
        : base(roleStore)
    {
    }

   
    public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
    {
        //return new ApplicationRoleManager(new ApplicationRoleStore(new ApplicationDbContext()));
        return new ApplicationRoleManager(new ApplicationRoleStore(context.Get<ApplicationDbContext>()));
        //return new ApplicationRoleManager(
        //    new RoleStore<ApplicationRole>(context.Get<ApplicationDbContext>()));
    }
}

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("NicePictureDBConfigure")
           // : base("NicePictureDBConfigure", throwIfV1Schema: false)
        {
            //this.Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<IdentityUser>().ToTable("Employee").Property(p => p.Id).HasColumnName("EmployeeId");
           modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            var appUser = modelBuilder.Entity<ApplicationUser>().ToTable("Employee").HasKey(p=>p.Id);
            //appUser.Property(p => p.Id).HasColumnName("EmployeeId");
            //appUser.Property(p => p.Name).HasColumnName("Name");
            //appUser.Property(p => p.Address).HasColumnName("Address");
            //appUser.Property(p => p.Position).HasColumnName("Position");
            //appUser.Property(p => p.StartDate).HasColumnName("StartDate");
            //appUser.Property(p => p.ManagerId).HasColumnName("ManagerId").IsRequired();
            //appUser.Property(p => p.Status).HasColumnName("Status").IsRequired();
            //appUser.Property(p => p.IdentificationNumber).HasColumnName("IdentificationNumber").IsRequired();
            //appUser.Property(p => p.PhoneNumber).HasColumnName("PhoneNumber");
            //appUser.Property(p => p.State).HasColumnName("State");
            //appUser.Property(p => p.PostalCode).HasColumnName("PostalCode");
            //appUser.Property(p => p.City).HasColumnName("City");
            //appUser.Property(p => p.Education).HasColumnName("Education");
            //appUser.Property(p => p.Specialability).HasColumnName("Specialability");
            //appUser.Property(p => p.Email).HasColumnName("Email");
            //appUser.Property(p => p.UserName).HasColumnName("UserName").IsRequired();


            //appUser.HasMany(u => u.Claims).WithRequired().HasForeignKey(k => k.UserId);
            //appUser.HasMany(u => u.Logins).WithRequired().HasForeignKey(k => k.UserId);
            //appUser.HasMany(u => u.Roles).WithRequired().HasForeignKey(k => k.RoleId);
            //appUser.HasMany(u => u.EmployeeSchedules).WithRequired().HasForeignKey(k => k.);
            //appUser.HasMany(u => u.EmployeeStatuses).WithRequired().HasForeignKey(k => k.Id);


            var empRole = modelBuilder.Entity<ApplicationUserRole>().ToTable("EmployeeRole").HasKey(p => new { p.UserId, p.RoleId });
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("EmployeeLogin").HasKey(p => new { p.LoginProvider, p.ProviderKey, p.UserId });
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("EmployeeClaim");
            var role = modelBuilder.Entity<ApplicationRole>().HasKey(p => p.Id).ToTable("Role");
           // role.HasMany(u => u.Users).WithRequired().HasForeignKey(ur => ur.RoleId);

           // modelBuilder.Entity<ApplicationUser>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
           // modelBuilder.Entity<ApplicationUser>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
           // modelBuilder.Entity<ApplicationRole>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

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

       // public System.Data.Entity.DbSet<NicePictureStudio.App_Data.Service> Services { get; set; }

        //public System.Data.Entity.DbSet<NicePictureStudio.App_Data.Customer> Customers { get; set; }
    }
}