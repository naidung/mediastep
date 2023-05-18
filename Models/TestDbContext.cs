using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestProject.Models;

public partial class TestDbContext : DbContext
{
    public TestDbContext()
    {
    }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<ManagerEmployee2> ManagerEmployee2s { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmpId);

            entity.ToTable("Employee");

            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsFixedLength();
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.ToTable("Manager");

            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsFixedLength();
        });

        modelBuilder.Entity<ManagerEmployee2>(entity =>
        {
            entity.HasKey(e => e.AutoId);

            entity.ToTable("Manager_Employee2");

            entity.HasOne(d => d.Emp).WithMany(p => p.ManagerEmployee2s)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Manager_Employee2_Employee");

            entity.HasOne(d => d.Manager).WithMany(p => p.ManagerEmployee2s)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Manager_Employee2_Manager");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
