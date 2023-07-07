// Purpose: Contains the AppDbContext class which is used to connect to the database.

namespace OnboardingApp.Model
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProviderAddress> ProviderAddresses { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserProvider> UserProviders { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationHistory> ConversationHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map entities to tables  
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Address>().ToTable("address");
            modelBuilder.Entity<Provider>().ToTable("provider");
            modelBuilder.Entity<Conversation>().ToTable("conversation");

            modelBuilder.Entity<UserAddress>().ToTable("useraddress");
            modelBuilder.Entity<UserProvider>().ToTable("userprovider");
            
            modelBuilder.Entity<ConversationHistory>().ToTable("conversationhistory");
            modelBuilder.Entity<ProviderAddress>().ToTable("provideraddress");

            // Configure Primary Keys
            modelBuilder.Entity<User>().HasKey(u => u.ID).HasName("pk_user");
            modelBuilder.Entity<Address>().HasKey(a => a.Id).HasName("pk_address");
            modelBuilder.Entity<Provider>().HasKey(p => p.ID).HasName("pk_provider");
            modelBuilder.Entity<Conversation>().HasKey(c => c.ID).HasName("pk_conversation");

            modelBuilder.Entity<UserAddress>().HasKey(ua => ua.ID).HasName("pk_useraddress");
            modelBuilder.Entity<UserProvider>().HasKey(up => up.ID).HasName("pk_userprovider");

            modelBuilder.Entity<ConversationHistory>().HasKey(ch => ch.ID).HasName("pk_conversationhistory");
            modelBuilder.Entity<ProviderAddress>().HasKey(pa => pa.ID).HasName("pk_provideraddress");

            // Configure columns
            modelBuilder.Entity<User>().Property(u => u.ID).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<User>().Property(u => u.FirstName).HasColumnType("varchar(255)").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.LastName).HasColumnType("varchar(255)");
            modelBuilder.Entity<User>().Property(u => u.Email).HasColumnType("varchar(255)").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Password).HasColumnType("varchar(255)").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Token).HasColumnType("varchar(2000)");

            modelBuilder.Entity<Address>().Property(a => a.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Address>().Property(a => a.Street1).HasColumnType("varchar(255)");
            modelBuilder.Entity<Address>().Property(a => a.City).HasColumnType("varchar(255)");
            modelBuilder.Entity<Address>().Property(a => a.Street2).HasColumnType("varchar(255)");
            modelBuilder.Entity<Address>().Property(a => a.Zip).HasColumnType("varchar(255)");

            modelBuilder.Entity<Provider>().Property(p => p.ID).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Provider>().Property(p => p.Name).HasColumnType("varchar(255)").IsRequired();

            modelBuilder.Entity<Conversation>().Property(c => c.ID).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Conversation>().Property(c => c.UserId).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Conversation>().Property(c => c.ProviderId).HasColumnType("int").IsRequired();

            modelBuilder.Entity<UserAddress>().Property(ua => ua.ID).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<UserAddress>().Property(ua => ua.UserId).HasColumnType("int").IsRequired();
            modelBuilder.Entity<UserAddress>().Property(ua => ua.AddressId).HasColumnType("int").IsRequired();

            modelBuilder.Entity<UserProvider>().Property(up => up.ID).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<UserProvider>().Property(up => up.UserId).HasColumnType("int").IsRequired();
            modelBuilder.Entity<UserProvider>().Property(up => up.ProviderId).HasColumnType("int").IsRequired();

            modelBuilder.Entity<ConversationHistory>().Property(ch => ch.ID).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<ConversationHistory>().Property(ch => ch.ConvId).HasColumnType("int").IsRequired();
            modelBuilder.Entity<ConversationHistory>().Property(ch => ch.Question).HasColumnType("varchar(255)");
            modelBuilder.Entity<ConversationHistory>().Property(ch => ch.Answer).HasColumnType("varchar(255)");

            modelBuilder.Entity<ProviderAddress>().Property(pa => pa.ID).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<ProviderAddress>().Property(pa => pa.ProviderId).HasColumnType("int").IsRequired();
            modelBuilder.Entity<ProviderAddress>().Property(pa => pa.AddressId).HasColumnType("int").IsRequired();

            // Configure relationships
            modelBuilder.Entity<UserAddress>().HasOne(ua => ua.User).WithMany(u => u.UserAddresses).HasForeignKey(ua => ua.UserId).HasConstraintName("fk_useraddress_user");
            modelBuilder.Entity<UserAddress>().HasOne(ua => ua.Address).WithMany(a => a.UserAddresses).HasForeignKey(ua => ua.AddressId).HasConstraintName("fk_useraddress_address");

            modelBuilder.Entity<UserProvider>().HasOne(up => up.User).WithMany(u => u.UserProviders).HasForeignKey(up => up.UserId).HasConstraintName("fk_userprovider_user");
            modelBuilder.Entity<UserProvider>().HasOne(up => up.Provider).WithMany(p => p.UserProviders).HasForeignKey(up => up.ProviderId).HasConstraintName("fk_userprovider_provider");

            modelBuilder.Entity<Conversation>().HasOne(c => c.User).WithMany(u => u.Conversations).HasForeignKey(c => c.UserId).HasConstraintName("fk_conversation_user");
            modelBuilder.Entity<Conversation>().HasOne(c => c.Provider).WithMany(p => p.Conversations).HasForeignKey(c => c.ProviderId).HasConstraintName("fk_conversation_provider");

            modelBuilder.Entity<ConversationHistory>().HasOne(ch => ch.Conversation).WithMany(c => c.ConversationHistories).HasForeignKey(ch => ch.ConvId).HasConstraintName("fk_conversationhistory_conversation");

            modelBuilder.Entity<ProviderAddress>().HasOne(pa => pa.Provider).WithMany(p => p.ProviderAddresses).HasForeignKey(pa => pa.ProviderId).HasConstraintName("fk_provideraddress_provider");
            modelBuilder.Entity<ProviderAddress>().HasOne(pa => pa.Address).WithMany(a => a.ProviderAddresses).HasForeignKey(pa => pa.AddressId).HasConstraintName("fk_provideraddress_address");

        }
    }
}