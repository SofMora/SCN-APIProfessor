using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APIProfessor.Models;

public partial class ScnAppointmentsDbContext : DbContext
{
    public ScnAppointmentsDbContext()
    {
    }

    public ScnAppointmentsDbContext(DbContextOptions<ScnAppointmentsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<CommentConsult> CommentConsults { get; set; }

    public virtual DbSet<Consult> Consults { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<ScheduleProfessor> ScheduleProfessors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=52.186.174.4;Database=SCN_AppointmentsDB;User Id=lenguajes;Password=Lenguajes2025$;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.ToTable("Appointment", "scn_appointments");

            entity.Property(e => e.CommentStatus).HasMaxLength(100);
            entity.Property(e => e.DateAppointment).HasColumnType("datetime");
            entity.Property(e => e.DescriptionAppointment).HasMaxLength(200);
            entity.Property(e => e.SubjectAppointment).HasMaxLength(50);

            entity.HasOne(d => d.IdScheduleNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.IdSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointment_ScheduleProfessor");
        });

        modelBuilder.Entity<CommentConsult>(entity =>
        {
            entity.ToTable("CommentConsult", "scn_appointments");

            entity.Property(e => e.DateComment).HasColumnType("datetime");
            entity.Property(e => e.DescriptionComment).HasMaxLength(500);

            entity.HasOne(d => d.IdConsultNavigation).WithMany(p => p.CommentConsults)
                .HasForeignKey(d => d.IdConsult)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommentConsult_Consult");
        });

        modelBuilder.Entity<Consult>(entity =>
        {
            entity.ToTable("Consult", "scn_appointments");

            entity.Property(e => e.DateConsult).HasColumnType("datetime");
            entity.Property(e => e.DescriptionConsult).HasMaxLength(500);
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ErrorLog__3214EC072A2D1E2D");

            entity.ToTable("ErrorLog", "scn_appointments");

            entity.Property(e => e.ErrorDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage).HasMaxLength(4000);
            entity.Property(e => e.ProcedureName).HasMaxLength(100);
        });

        modelBuilder.Entity<ScheduleProfessor>(entity =>
        {
            entity.ToTable("ScheduleProfessor", "scn_appointments");

            entity.Property(e => e.Day).HasMaxLength(50);
            entity.Property(e => e.Time).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
