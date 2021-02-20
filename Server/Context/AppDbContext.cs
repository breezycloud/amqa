using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AlfalahApp.Server.Models;

#nullable disable

namespace AlfalahApp.Server.Context
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EmployeeDetail> EmployeeDetails { get; set; }
        public virtual DbSet<PaySlip> PaySlips { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<EmployeeDetail>(entity =>
            {
                entity.ToTable("EmployeeDetail");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Allowance).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Basic).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Dept)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.PhoneNumber1)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.PhoneNumber2)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.Rent).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Tax).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Transport).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<PaySlip>(entity =>
            {
                entity.ToTable("PaySlip");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Empid)
                    .HasColumnName("empid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Iou)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("IOU");

                entity.Property(e => e.Loans).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.OtherDeduction).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.OtherEarning).HasColumnType("numeric(18, 2)");                

                entity.Property(e => e.Paydate)
                    .HasPrecision(0)
                    .HasColumnName("paydate");

                entity.Property(e => e.Term)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("term");

                entity.Property(e => e.Note)
                      .HasColumnName("Note");  
                
                entity.Property(e => e.AcdSession)
                      .HasMaxLength(4)
                      .HasColumnName("AcdSession");  

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.PaySlips)
                    .HasForeignKey(d => d.Empid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PaySlip__id__2B3F6F97");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Passwordhash)
                    .IsRequired()
                    .HasColumnName("passwordhash");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
