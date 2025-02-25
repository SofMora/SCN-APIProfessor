using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APIProfessor.Models;

public partial class ScnAdminDbContext : DbContext
{
    public ScnAdminDbContext()
    {
    }

    public ScnAdminDbContext(DbContextOptions<ScnAdminDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<TypeNews> TypeNews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=52.186.174.4;Database=SCN_AdminDB;User Id=lenguajes;Password=Lenguajes2025$;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("Admin", "scn_admin");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment", "scn_admin");

            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasOne(d => d.IdNewsNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdNews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_News");
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ErrorLog__3214EC0742888D04");

            entity.ToTable("ErrorLog", "scn_admin");

            entity.Property(e => e.ErrorDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage).HasMaxLength(4000);
            entity.Property(e => e.ProcedureName).HasMaxLength(100);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.ToTable("News", "scn_admin");

            entity.Property(e => e.DateNews).HasColumnType("datetime");
            entity.Property(e => e.TextNews).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.TypeNewsNavigation).WithMany(p => p.News)
                .HasForeignKey(d => d.TypeNews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_News_TypeNews");
        });

        modelBuilder.Entity<TypeNews>(entity =>
        {
            entity.ToTable("TypeNews", "scn_admin");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
