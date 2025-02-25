using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APIProfessor.Models;

public partial class ScnProfessorDbContext : DbContext
{
    public ScnProfessorDbContext()
    {
    }

    public ScnProfessorDbContext(DbContextOptions<ScnProfessorDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Professor> Professors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=52.186.174.4;Database=SCN_ProfessorDB;User Id=lenguajes;Password=Lenguajes2025$;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course", "scn_professor");

            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.IdProfessorNavigation).WithMany(p => p.Courses)
                .HasForeignKey(d => d.IdProfessor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_Professors");
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ErrorLog__3214EC07D3B0558F");

            entity.ToTable("ErrorLog", "scn_professor");

            entity.Property(e => e.ErrorDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage).HasMaxLength(4000);
            entity.Property(e => e.ProcedureName).HasMaxLength(100);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable("Group", "scn_professor");

            entity.HasOne(d => d.IdCourseNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.IdCourse)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Group_Course");

            entity.HasOne(d => d.IdProfessorNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.IdProfessor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Group_Professors");
        });

        modelBuilder.Entity<Professor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Professor");

            entity.ToTable("Professors", "scn_professor");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.SocialLink).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
