using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.DAL.Models;

public partial class ExpenseTrackerContext : DbContext
{
    public ExpenseTrackerContext()
    {
    }

    public ExpenseTrackerContext(DbContextOptions<ExpenseTrackerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppSetting> AppSettings { get; set; }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<AppUserPermission> AppUserPermissions { get; set; }

    public virtual DbSet<AuditTrail> AuditTrails { get; set; }

    public virtual DbSet<CalendarEvent> CalendarEvents { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Functionality> Functionalities { get; set; }

    public virtual DbSet<OrgUnit> OrgUnits { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SecurityRole> SecurityRoles { get; set; }

    public virtual DbSet<SecurityRoleFunctionality> SecurityRoleFunctionalities { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ANIL-XPS;Database=ExpenseTracker;user id=NaviCare; password=NaviCare;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppSetting>(entity =>
        {
            entity.ToTable("AppSetting", "config", tb => tb.HasComment("Common setting values for application"));

            entity.Property(e => e.AppSettingID).HasComment("Primary Key Identifier of this table.");
            entity.Property(e => e.AppSettingName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Configuration Name used in the applications");
            entity.Property(e => e.AppSettingValue).HasComment("Configuration Values for curresponding names");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(5000)
                .IsUnicode(false)
                .HasComment("Description about the configuration Name and Value");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.UserID);

            entity.ToTable("AppUser");

            entity.Property(e => e.ContactEmail).HasMaxLength(50);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastAccessedOn).HasColumnType("datetime");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.PasswordResetOn).HasColumnType("datetime");
            entity.Property(e => e.ProfilePictureURL)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.DefaultOrgUnit).WithMany(p => p.AppUsers)
                .HasForeignKey(d => d.DefaultOrgUnitID)
                .HasConstraintName("FK_AppUser_OrgUnit");

            entity.HasOne(d => d.DefaultSecurityRole).WithMany(p => p.AppUsers)
                .HasForeignKey(d => d.DefaultSecurityRoleID)
                .HasConstraintName("FK_AppUser_SecurityRole");
        });

        modelBuilder.Entity<AppUserPermission>(entity =>
        {
            entity.ToTable("AppUserPermission");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.OrgUnit).WithMany(p => p.AppUserPermissions)
                .HasForeignKey(d => d.OrgUnitID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserPermission_OrgUnit");

            entity.HasOne(d => d.SecurityRole).WithMany(p => p.AppUserPermissions)
                .HasForeignKey(d => d.SecurityRoleID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserPermission_SecurityRole");
        });

        modelBuilder.Entity<AuditTrail>(entity =>
        {
            entity.HasKey(e => e.AuditID);

            entity.ToTable("AuditTrail");

            entity.Property(e => e.AuditText)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IPAddress)
                .HasMaxLength(20)
                .IsFixedLength();

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AuditTrails)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuditTrail_AppUser");

            entity.HasOne(d => d.OrgUnit).WithMany(p => p.AuditTrails)
                .HasForeignKey(d => d.OrgUnitID)
                .HasConstraintName("FK_AuditTrail_OrgUnit");

            entity.HasOne(d => d.Vendor).WithMany(p => p.AuditTrails)
                .HasForeignKey(d => d.VendorID)
                .HasConstraintName("FK_AuditTrail_Vendor");
        });

        modelBuilder.Entity<CalendarEvent>(entity =>
        {
            entity.ToTable("CalendarEvent");

            entity.Property(e => e.AssignedStaffs)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EventEndDateTime).HasColumnType("datetime");
            entity.Property(e => e.EventStartDateTime).HasColumnType("datetime");
            entity.Property(e => e.EventTitle)
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country", "config");

            entity.Property(e => e.CountryID).ValueGeneratedNever();
            entity.Property(e => e.CountryName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.ToTable("Document");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DocumentName).HasMaxLength(500);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.BankName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ContactAddress)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ContactEmail).HasMaxLength(50);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.ContactPostalCode)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Designation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IBANNumber)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.IDExpiresOn).HasColumnType("datetime");
            entity.Property(e => e.IDIssuedOn).HasColumnType("datetime");
            entity.Property(e => e.IDNumber)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.JoinDate).HasColumnType("datetime");
            entity.Property(e => e.LabourCardExpiresOn).HasColumnType("datetime");
            entity.Property(e => e.LabourCardIssuedOn).HasColumnType("datetime");
            entity.Property(e => e.LabourCardNumber)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PassportExpiresOn).HasColumnType("datetime");
            entity.Property(e => e.PassportIssuedOn).HasColumnType("datetime");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.PermanentAddress)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.PermanentPostalCode)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ProfilePictureURL)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Employees)
                .HasForeignKey(d => d.UserID)
                .HasConstraintName("FK_Employee_AppUser");
        });

        modelBuilder.Entity<Functionality>(entity =>
        {
            entity.HasKey(e => e.FunctionalityID).HasName("PK_Functionalities");

            entity.ToTable("Functionality", "config");

            entity.Property(e => e.FunctionalityID).ValueGeneratedNever();
            entity.Property(e => e.FunctionalityName)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OrgUnit>(entity =>
        {
            entity.ToTable("OrgUnit");

            entity.Property(e => e.AuthorisedSignatureURL)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.AuthorisedStampSealURL)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BillingAddressLine1)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BillingAddressLine2)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BillingAddressLine3)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ContactAddress)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ContactEmail).HasMaxLength(50);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.ContactPerson).HasMaxLength(50);
            entity.Property(e => e.ContactPostalCode)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.LogoURL)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrgUnitName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TRN)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.Website)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Country).WithMany(p => p.OrgUnits)
                .HasForeignKey(d => d.CountryID)
                .HasConstraintName("FK_OrgUnit_Country");

            entity.HasOne(d => d.State).WithMany(p => p.OrgUnits)
                .HasForeignKey(d => d.StateID)
                .HasConstraintName("FK_OrgUnit_State");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.ToTable("Purchase");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentRemark).HasMaxLength(500);
            entity.Property(e => e.PurchasedBy).HasMaxLength(100);
            entity.Property(e => e.PurchasedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.VendorID)
                .HasConstraintName("FK_Purchase_Vendor");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SalesID);

            entity.ToTable("Sale");

            entity.Property(e => e.BankAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CashAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreditAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SalesOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<SecurityRole>(entity =>
        {
            entity.ToTable("SecurityRole");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SecurityRoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<SecurityRoleFunctionality>(entity =>
        {
            entity.ToTable("SecurityRoleFunctionality");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Functionality).WithMany(p => p.SecurityRoleFunctionalities)
                .HasForeignKey(d => d.FunctionalityID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SecurityRoleFunctionality_Functionality");

            entity.HasOne(d => d.SecurityRole).WithMany(p => p.SecurityRoleFunctionalities)
                .HasForeignKey(d => d.SecurityRoleID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SecurityRoleFunctionality_SecurityRole");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.ToTable("State", "config");

            entity.Property(e => e.StateName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_State_Country");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.ToTable("Vendor");

            entity.Property(e => e.BillingAddress)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.BillingName).HasMaxLength(250);
            entity.Property(e => e.ContactAddress)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ContactEmail).HasMaxLength(50);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.ContactPostalCode)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProfilePictureURL)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TRN)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.VendorCode)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.VendorName)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.Website)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Country).WithMany(p => p.Vendors)
                .HasForeignKey(d => d.CountryID)
                .HasConstraintName("FK_Vendor_Country");

            entity.HasOne(d => d.DefaultOrgUnit).WithMany(p => p.Vendors)
                .HasForeignKey(d => d.DefaultOrgUnitID)
                .HasConstraintName("FK_Vendor_OrgUnit");

            entity.HasOne(d => d.ParentVendor).WithMany(p => p.InverseParentVendor)
                .HasForeignKey(d => d.ParentVendorID)
                .HasConstraintName("FK_Vendor_Vendor");

            entity.HasOne(d => d.State).WithMany(p => p.Vendors)
                .HasForeignKey(d => d.StateID)
                .HasConstraintName("FK_Vendor_State");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
