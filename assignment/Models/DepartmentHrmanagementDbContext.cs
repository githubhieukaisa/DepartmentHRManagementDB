using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace assignment.Models;

public partial class DepartmentHrmanagementDbContext : DbContext
{
    public DepartmentHrmanagementDbContext()
    {
    }

    public DepartmentHrmanagementDbContext(DbContextOptions<DepartmentHrmanagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeProject> EmployeeProjects { get; set; }

    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectProgress> ProjectProgresses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var congfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(congfig.GetConnectionString("DBContext"));
        }

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BEDAF0AE7FD");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasMaxLength(250);

            entity.HasOne(d => d.Manager).WithMany(p => p.Departments)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_Department_Manager");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F1121B08F64");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.Email, "UQ__Employee__A9D105343D209F4A").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Employee__Depart__5535A963");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Employee__RoleId__5441852A");
        });

        modelBuilder.Entity<EmployeeProject>(entity =>
        {
            entity.HasKey(e => e.EmployeeProjectId).HasName("PK__Employee__541BC8B117A4A8FC");

            entity.ToTable("EmployeeProject");

            entity.Property(e => e.AssignedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeProjects)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__EmployeeP__Emplo__60A75C0F");

            entity.HasOne(d => d.Project).WithMany(p => p.EmployeeProjects)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__EmployeeP__Proje__619B8048");
        });

        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.HasKey(e => e.LeaveRequestId).HasName("PK__LeaveReq__609421EEBB5D3DAB");

            entity.ToTable("LeaveRequest");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Reason).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Employee).WithMany(p => p.LeaveRequestEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__LeaveRequ__Emplo__59063A47");

            entity.HasOne(d => d.Manager).WithMany(p => p.LeaveRequestManagers)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__LeaveRequ__Manag__5AEE82B9");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Project__761ABEF01C04C711");

            entity.ToTable("Project");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ProjectName).HasMaxLength(100);
        });

        modelBuilder.Entity<ProjectProgress>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("PK__ProjectP__BAE29CA5AA9C2836");

            entity.ToTable("ProjectProgress");

            entity.Property(e => e.ProgressDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ProgressNote).HasMaxLength(255);

            entity.HasOne(d => d.Employee).WithMany(p => p.ProjectProgresses)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__ProjectPr__Emplo__656C112C");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectProgresses)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__ProjectPr__Proje__66603565");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1A79860B25");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
