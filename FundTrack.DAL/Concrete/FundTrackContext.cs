﻿using FundTrack.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FundTrack.DAL.Concrete
{
    /// <summary>
    /// EF Database context
    /// </summary>
    public class FundTrackContext : DbContext
    {
        public FundTrackContext(DbContextOptions<FundTrackContext> options)
            : base(options) { }

        /// <summary>
        /// Gets or Sets Users
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or Sets Addresses
        /// </summary>
        public DbSet<Address> Addresses { get; set; }

        /// <summary>
        /// Gets or Sets OrgAddresses
        /// </summary>
        public DbSet<OrgAddress> OrgAddresses { get; set; }

        /// <summary>
        /// Gets or Sets Roles
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Gets or Sets Phones
        /// </summary>
        public DbSet<Phone> Phones { get; set; }

        /// <summary>
        /// Gets or Sets ExtContacts
        /// </summary>
        public DbSet<ExternalContact> ExternalContacts { get; set; }

        /// <summary>
        /// Gets or Sets Ext Services
        /// </summary>
        public DbSet<ExternalService> ExternalServices { get; set; }

        /// <summary>
        /// Gets or Sets Organizations
        /// </summary>
        public DbSet<Organization> Organizations { get; set; }

        /// <summary>
        /// Gets or Sets Membershipes
        /// </summary>
        public DbSet<Membership> Membershipes { get; set; }

        /// <summary>
        /// Gets or Sets UserAddresses
        /// </summary>
        public DbSet<UserAddress> UserAddresses { get; set; }

        /// <summary>
        /// Gets or Sets BankAccounts
        /// </summary>
        public DbSet<BankAccount> BankAccounts { get; set; }

        /// <summary>
        /// Gets or Sets OrgAccounts
        /// </summary>
        public DbSet<OrgAccount> OrgAccounts { get; set; }

        /// <summary>
        /// Gets or Sets Balances
        /// </summary>
        public DbSet<Balance> Balances { get; set; }

        /// <summary>
        /// Gets or Sets Currencies
        /// </summary>
        public DbSet<Currency> Currencies { get; set; }

        /// <summary>
        /// Gets or Sets FinOps
        /// </summary>
        public DbSet<FinOp> FinOps { get; set; }

        /// <summary>
        /// Gets or Sets Tags
        /// </summary>
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// Gets or Sets TagFinOps
        /// </summary>
        public DbSet<TagFinOp> TagFinOps { get; set; }

        /// <summary>
        /// Gets or Sets Targets
        /// </summary>
        public DbSet<Target> Targets { get; set; }

        /// <summary>
        /// Gets or sets the bank imports.
        /// </summary>
        /// <value>
        /// The bank imports.
        /// </value>
        public DbSet<BankImport> BankImports { get; set; }

        /// <summary>
        /// Gets or sets the bank import details.
        /// </summary>
        /// <value>
        /// The bank import details.
        /// </value>
        public DbSet<BankImportDetail> BankImportDetails { get; set; }

        /// <summary>
        /// Gets or sets the complaints.
        /// </summary>
        /// <value>
        /// The complaints.
        /// </value>
        public DbSet<Complaint> Complaints { get; set; }

        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        /// <value>
        /// The events.
        /// </value>
        public DbSet<Event> Events { get; set; }

        /// <summary>
        /// Gets or sets the goods types.
        /// </summary>
        /// <value>
        /// The goods types.
        /// </value>
        public DbSet<GoodsType> GoodsTypes { get; set; }

        /// <summary>
        /// Gets or sets the goods categorys.
        /// </summary>
        /// <value>
        /// The goods categorys.
        /// </value>
        public DbSet<GoodsCategory> GoodsCategorys { get; set; }

        /// <summary>
        /// Gets or sets the offers.
        /// </summary>
        /// <value>
        /// The offers.
        /// </value>
        public DbSet<Offer> Offers { get; set; }

        /// <summary>
        /// Gets or sets the offered items.
        /// </summary>
        /// <value>
        /// The offered items.
        /// </value>
        public DbSet<OfferedItem> OfferedItems { get; set; }

        /// <summary>
        /// Gets or sets the requests.
        /// </summary>
        /// <value>
        /// The requests.
        /// </value>
        public DbSet<Request> Requests { get; set; }

        /// <summary>
        /// Gets or sets the requested items.
        /// </summary>
        /// <value>
        /// The requested items.
        /// </value>
        public DbSet<RequestedItem> RequestedItems { get; set; }

        /// <summary>
        /// Configures model creation
        /// </summary>
        /// <param name="modelBuilder">modelBuilder to configure Model Creation</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_User");

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Login).IsRequired().HasMaxLength(100);

                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Password).IsRequired().HasMaxLength(50);

                entity.Property(e => e.SaltKey).IsRequired().HasMaxLength(10);

                entity.Property(e => e.IsBanned).IsRequired();
            });

            modelBuilder.Entity<Phone>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Phone");

                entity.Property(e => e.Number).IsRequired().HasMaxLength(15);

                entity.Property(e => e.PhoneType).IsRequired().HasMaxLength(20);

                entity.HasOne(p => p.User)
                      .WithMany(u => u.Phones)
                      .HasForeignKey(p => p.UserId)
                      .HasConstraintName("FK_Phone_User");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Address");

                entity.Property(e => e.City).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Country).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Street).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<UserAddress>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_UserAddress");

                entity.HasOne(ua => ua.User)
                      .WithMany(u => u.UserAddresses)
                      .HasForeignKey(ua => ua.UserId)
                      .HasConstraintName("FK_UserAddress_User");

                entity.HasOne(ua => ua.Address)
                      .WithMany(a => a.UserAddresses)
                      .HasForeignKey(a => a.AddressId)
                      .HasConstraintName("FK_UserAddress_Address");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Role");

                entity.Property(e => e.Name).IsRequired().HasMaxLength(20);
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Organization");

                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                entity.Property(e => e.IsBanned).IsRequired();
            });

            modelBuilder.Entity<OrgAddress>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_OrgAddress");

                entity.HasOne(oa => oa.Organization)
                      .WithMany(o => o.OrgAddresses)
                      .HasForeignKey(oa => oa.OrgId)
                      .HasConstraintName("FK_OrgAddress_Organization");

                entity.HasOne(oa => oa.Address)
                      .WithMany(a => a.OrgAddresses)
                      .HasForeignKey(oa => oa.AddressId)
                      .HasConstraintName("FK_OrgAddress_Address");
            });

            modelBuilder.Entity<ExternalContact>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_ExternalContact");

                entity.Property(e => e.ServiceLogin).IsRequired().HasMaxLength(100);

                entity.HasOne(ec => ec.User)
                      .WithMany(u => u.ExternalContacts)
                      .HasForeignKey(ec => ec.UserId)
                      .HasConstraintName("FK_ExternalContact_User");

                entity.HasOne(ec => ec.ExternalService)
                      .WithMany(es => es.ExtContacts)
                      .HasForeignKey(ec => ec.ServiceId)
                      .HasConstraintName("FK_ExternalContact_ExternalService");
            });

            modelBuilder.Entity<Membership>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Membership");

                entity.HasOne(m => m.User)
                      .WithMany(u => u.Memberships)
                      .HasForeignKey(m => m.UserId)
                      .HasConstraintName("FK_Membership_User");

                entity.HasOne(m => m.Organization)
                      .WithMany(o => o.Memberships)
                      .HasForeignKey(m => m.OrgId)
                      .HasConstraintName("FK_Membership_Organization");

                entity.HasOne(m => m.Role)
                      .WithMany(r => r.Memberships)
                      .HasForeignKey(m => m.RoleId)
                      .HasConstraintName("FK_Membership_Role");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Currency");

                entity.Property(e => e.ShortName).IsRequired().HasMaxLength(5);

                entity.Property(e => e.FullName).IsRequired().HasMaxLength(30);
            });

            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_BankAccount");

                entity.Property(e => e.AccNumber).IsRequired().HasMaxLength(20);

                entity.Property(e => e.MFO).IsRequired().HasMaxLength(6);

                entity.Property(e => e.EDRPOU).IsRequired().HasMaxLength(10);

                entity.Property(e => e.BankName).IsRequired().HasMaxLength(50);

                entity.HasOne(ba => ba.Organization)
                      .WithMany(o => o.BankAccounts)
                      .HasForeignKey(ba => ba.OrgId)
                      .HasConstraintName("FK_BankAccount_Organization");
            });

            modelBuilder.Entity<OrgAccount>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_OrgAccount");

                entity.Property(e => e.OrgAccountName).IsRequired().HasMaxLength(100);

                entity.Property(e => e.AccountType).IsRequired().HasMaxLength(10);

                entity.Property(e => e.CurrentBalance).HasColumnType("decimal(18,2)");

                entity.HasOne(oa => oa.Organization)
                      .WithMany(o => o.OrgAccounts)
                      .HasForeignKey(oa => oa.OrgId)
                      .HasConstraintName("FK_OrgAccount_Organization");

                entity.HasOne(oa => oa.BankAccount)
                      .WithMany(ba => ba.OrgAccounts)
                      .HasForeignKey(oa => oa.BankAccId)
                      .HasConstraintName("FK_OrgAccount_BankAccount");

                entity.HasOne(oa => oa.Currency)
                      .WithMany(c => c.OrgAccounts)
                      .HasForeignKey(oa => oa.CurrencyId)
                      .HasConstraintName("FK_OrgAccount_Currency");
            });

            modelBuilder.Entity<Balance>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Balance");

                entity.Property(e => e.BalanceDate).HasColumnType("datetime");

                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");

                entity.HasOne(b => b.OrgAccount)
                      .WithMany(oa => oa.Balances)
                      .HasForeignKey(b => b.OrgAccountId)
                      .HasConstraintName("FK_Balance_OrgAccount");
            });

            modelBuilder.Entity<Target>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Target");

                entity.Property(e => e.TargetName).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<FinOp>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_FinOp");

                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");

                entity.Property(e => e.FinOpDate).HasColumnType("datetime");

                entity.HasOne(fo => fo.Target)
                      .WithOne(t => t.FinOp)
                      .HasForeignKey<FinOp>(fo => fo.TargetId)
                      .HasConstraintName("FK_FinOp_Target");

                entity.HasOne(fo => fo.OrgAccountFrom)
                      .WithMany(oa => oa.FinOpsFrom)
                      .HasForeignKey(fo => fo.AccFromId)
                      .HasConstraintName("FK_FinOp_OrgAccountFrom");

                entity.HasOne(fo => fo.OrgAccountTo)
                      .WithMany(oa => oa.FinOpsTo)
                      .HasForeignKey(fo => fo.AccToId)
                      .HasConstraintName("FK_FinOp_OrgAccountTo");

                entity.HasOne(fo => fo.User)
                      .WithMany(u => u.FinOps)
                      .HasForeignKey(fo => fo.UserId)
                      .HasConstraintName("FK_FinOp_User");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Tag");

                entity.Property(e => e.TagName).IsRequired().HasMaxLength(30);
            });

            modelBuilder.Entity<TagFinOp>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_TagFinOp");

                entity.HasOne(tfp => tfp.Tag)
                      .WithMany(t => t.TagFinOps)
                      .HasForeignKey(tfp => tfp.TagId)
                      .HasConstraintName("FK_TagFinOp_Tag");

                entity.HasOne(tfp => tfp.FinOp)
                      .WithMany(fo => fo.TagFinOps)
                      .HasForeignKey(tfp => tfp.FinOpId)
                      .HasConstraintName("FK_TagFinOp_FinOp");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Event");

                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);

                entity.Property(e => e.CreateDate).IsRequired().HasColumnType("datetime");

                entity.Property(e => e.ImageUrl).HasMaxLength(100);

                entity.HasOne(e => e.Organization)
                      .WithMany(o => o.Events)
                      .HasForeignKey(e => e.OrganizationId)
                      .HasConstraintName("FK_Event_Organization");
            });

            modelBuilder.Entity<Complaint>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Complaint");

                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);

                entity.Property(e => e.CreateDate).IsRequired().HasColumnType("datetime");

                entity.Property(e => e.IsLooked).IsRequired();

                entity.HasOne(c => c.User)
                      .WithMany(u => u.Complaints)
                      .HasForeignKey(c => c.UserId)
                      .HasConstraintName("FK_Complaint_User");

                entity.HasOne(c => c.Organization)
                      .WithMany(o => o.Complaints)
                      .HasForeignKey(c => c.OrganizationId)
                      .HasConstraintName("FK_Complaint_Organization");
            });

            modelBuilder.Entity<GoodsType>(entity =>
            {
                entity.HasKey(gt => gt.Id).HasName("PK_GoodsType");

                entity.Property(gt => gt.Name).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<GoodsCategory>(entity =>
            {
                entity.HasKey(gc => gc.Id).HasName("PK_GoodsCategory");

                entity.Property(gc => gc.Name).IsRequired().HasMaxLength(50);

                entity.HasOne(gc => gc.GoodsType)
                       .WithMany(gt => gt.GoodsCategories)
                       .HasForeignKey(gc => gc.GoodsTypeId)
                       .HasConstraintName("FK_GoodsCategory_GoodsType");
            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.HasKey(o => o.Id).HasName("PK_Offer");

                entity.Property(o => o.Description).IsRequired().HasMaxLength(500);

                entity.Property(o => o.CreateDate).IsRequired().HasColumnType("datetime");

                entity.Property(o => o.IsActual).IsRequired();

                entity.HasOne(o => o.User)
                       .WithMany(u => u.Offers)
                       .HasForeignKey(o => o.UserId)
                       .HasConstraintName("FK_Offer_User");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.HasKey(r => r.Id).HasName("PK_Request");

                entity.Property(r => r.Description).IsRequired().HasMaxLength(500);

                entity.Property(r => r.CreateDate).IsRequired().HasColumnType("datetime");

                entity.Property(r => r.IsActual).IsRequired();

                entity.HasOne(r => r.Organization)
                       .WithMany(o => o.Requests)
                       .HasForeignKey(o => o.OrganizationId)
                       .HasConstraintName("FK_Request_Organization");
            });

            modelBuilder.Entity<OfferedItem>(entity =>
            {
                entity.HasKey(oi => oi.Id).HasName("PK_OfferedItem");

                entity.Property(oi => oi.Name).IsRequired().HasMaxLength(50);

                entity.Property(oi => oi.Description).IsRequired().HasMaxLength(500);

                entity.Property(oi => oi.ImageUrl).HasMaxLength(100);

                entity.Property(oi => oi.IsActual).IsRequired();

                entity.HasOne(oi => oi.Offer)
                       .WithMany(o => o.OfferedItems)
                       .HasForeignKey(oi => oi.OfferId)
                       .HasConstraintName("FK_OfferedItems_Offer");

                entity.HasOne(oi => oi.GoodsCategory)
                       .WithMany(gc => gc.OfferedItems)
                       .HasForeignKey(oi => oi.GoodsCategoryId)
                       .HasConstraintName("FK_OfferedItems_GoodsCategory");
            });
            //
            modelBuilder.Entity<RequestedItem>(entity =>
            {
                entity.HasKey(ri => ri.Id).HasName("PK_RequestedItem");

                entity.Property(ri => ri.Name).IsRequired().HasMaxLength(50);

                entity.Property(ri => ri.Description).IsRequired().HasMaxLength(500);

                entity.Property(ri => ri.ImageUrl).HasMaxLength(100);

                entity.Property(ri => ri.IsActual).IsRequired();

                entity.HasOne(ri => ri.Request)
                       .WithMany(o => o.RequestedItems)
                       .HasForeignKey(ri => ri.RequestId)
                       .HasConstraintName("FK_RequestedItem_Request");

                entity.HasOne(ri => ri.GoodsCategory)
                       .WithMany(gc => gc.RequestedItems)
                       .HasForeignKey(ri => ri.GoodsCategoryId)
                       .HasConstraintName("FK_RequestedItem_GoodsCategory");
            });

            modelBuilder.Entity<BankImport>(entity =>
            {
                entity.HasKey(bi => bi.Id).HasName("PK_BankImport");

                entity.Property(bi => bi.IdMerchant).IsRequired();

                entity.Property(bi => bi.Signature).IsRequired();

                entity.Property(bi => bi.Credit).HasColumnType("decimal(18,2)");

                entity.Property(bi => bi.Debet).HasColumnType("decimal(18,2)");

            });

            modelBuilder.Entity<BankImportDetail>(entity =>
            {
                entity.HasKey(bid => bid.Id).HasName("PK_BankImportDetail");

                entity.Property(bid => bid.Card).IsRequired();

                entity.Property(bid => bid.Trandate).IsRequired().HasColumnType("datetime");

                entity.Property(bid => bid.Trantime).IsRequired().HasColumnType("datetime");

                entity.Property(bid => bid.Amount).IsRequired();

                entity.Property(bid => bid.CardAmount).IsRequired();

                entity.Property(bid => bid.Rest).IsRequired();

                entity.Property(bid => bid.Terminal).IsRequired();

                entity.Property(bid => bid.Description).IsRequired();

                entity.HasOne(bid => bid.BankImport)
                       .WithMany(bi => bi.BankImportDetails)
                       .HasForeignKey(bid => bid.BankImportId)
                       .HasConstraintName("FK_BankImportDetails_BankImport");
            });
        }
    }
}