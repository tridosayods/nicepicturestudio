﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NicePictureStudio.App_Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NicePictureStudioDBEntities : DbContext
    {
        public NicePictureStudioDBEntities()
            : base("name=NicePictureStudioDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<BookingStatu> BookingStatus { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<EmployeeClaim> EmployeeClaims { get; set; }
        public virtual DbSet<EmployeeLogin> EmployeeLogins { get; set; }
        public virtual DbSet<Equipment> Equipments { get; set; }
        public virtual DbSet<EquipmentSchedule> EquipmentSchedules { get; set; }
        public virtual DbSet<EquipmentService> EquipmentServices { get; set; }
        public virtual DbSet<EquipmentStatu> EquipmentStatus { get; set; }
        public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }
        public virtual DbSet<LocationSchedule> LocationSchedules { get; set; }
        public virtual DbSet<LocationService> LocationServices { get; set; }
        public virtual DbSet<LocationStatu> LocationStatus { get; set; }
        public virtual DbSet<OutputSchedule> OutputSchedules { get; set; }
        public virtual DbSet<OutputService> OutputServices { get; set; }
        public virtual DbSet<OutsourceContact> OutsourceContacts { get; set; }
        public virtual DbSet<OutsourceSchedule> OutsourceSchedules { get; set; }
        public virtual DbSet<OutsourceService> OutsourceServices { get; set; }
        public virtual DbSet<OutsourceServiceType> OutsourceServiceTypes { get; set; }
        public virtual DbSet<OutsourceStatu> OutsourceStatus { get; set; }
        public virtual DbSet<PhotographService> PhotographServices { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ServiceForm> ServiceForms { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceStatu> ServiceStatus { get; set; }
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }
        public virtual DbSet<EmployeeStatu> EmployeeStatus { get; set; }
        public virtual DbSet<EmployeeSchedule> EmployeeSchedules { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<OutputStatu> OutputStatus { get; set; }
        public virtual DbSet<PromotionStatu> PromotionStatus { get; set; }
        public virtual DbSet<CRMForm> CRMForms { get; set; }
        public virtual DbSet<CRMScrore> CRMScrores { get; set; }
        public virtual DbSet<CRMTemplate> CRMTemplates { get; set; }
        public virtual DbSet<CRMServiceCategory> CRMServiceCategories { get; set; }
        public virtual DbSet<CRMServiceType> CRMServiceTypes { get; set; }
        public virtual DbSet<BookingSpecialRequest> BookingSpecialRequests { get; set; }
        public virtual DbSet<ServiceSuggestion> ServiceSuggestions { get; set; }
        public virtual DbSet<ServiceFormLocation> ServiceFormLocations { get; set; }
        public virtual DbSet<LocationStyle> LocationStyles { get; set; }
        public virtual DbSet<LocationType> LocationTypes { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<EmployeeInfo> EmployeeInfoes { get; set; }
        public virtual DbSet<EmployeePosition> EmployeePositions { get; set; }
        public virtual DbSet<OutputSize> OutputSizes { get; set; }
        public virtual DbSet<OutputType> OutputTypes { get; set; }
    }
}
