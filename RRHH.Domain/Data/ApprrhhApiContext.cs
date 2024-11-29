using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RRHH.Domain.Entities;

namespace RRHH.Domain.Data;

public partial class ApprrhhApiContext : DbContext
{
    public ApprrhhApiContext()
    {
    }

    public ApprrhhApiContext(DbContextOptions<ApprrhhApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Leaf> Leaves { get; set; }

    public virtual DbSet<PerformanceReview> PerformanceReviews { get; set; }

    public virtual DbSet<TimeTable> TimeTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-4UIV9D8;Initial Catalog=APPRRHH_API;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC077B3A4EC8");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentName)
                .HasMaxLength(500)
                .HasColumnName("Department_Name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC0770E42811");

            entity.Property(e => e.FirstName).HasMaxLength(500);
            entity.Property(e => e.LastName).HasMaxLength(500);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Role).HasMaxLength(500);

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_Employees_Department");

            entity.HasOne(d => d.WorkingHours).WithMany(p => p.Employees)
                .HasForeignKey(d => d.WorkingHoursId)
                .HasConstraintName("FK_Employees_Time_Table");
        });

        modelBuilder.Entity<Leaf>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Leaves__3214EC07C1637505");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.LeaveType).HasMaxLength(500);
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(500);

            entity.HasOne(d => d.Employee).WithMany(p => p.Leaves)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Leaves_Employees");
        });

        modelBuilder.Entity<PerformanceReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Performa__3214EC07FE630ED2");

            entity.ToTable("Performance_Reviews");

            entity.Property(e => e.Comments).HasMaxLength(500);
            entity.Property(e => e.ReviewDate).HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.PerformanceReviews)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Performance_Reviews_Employees");
        });

        modelBuilder.Entity<TimeTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Time_Tab__3214EC07220F9D03");

            entity.ToTable("Time_Table");

            entity.Property(e => e.ScheduleType)
                .HasMaxLength(500)
                .HasColumnName("Schedule_Type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
