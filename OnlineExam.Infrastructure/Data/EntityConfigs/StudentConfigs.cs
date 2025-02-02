using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineExam.Domain.Entities;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.FullName)
            .HasColumnType("NVARCHAR(255)")
            .IsRequired();

        builder.Property(s => s.Gender)
            .HasColumnType("VARCHAR(6)")
            .IsRequired();

        builder.Property(s => s.SchoolYear)
            .IsRequired();

        builder.HasMany(s => s.Enrollments)
            .WithOne(e => e.Student)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.ApplicationUser)
            .WithOne(a => a.Student)
            .HasForeignKey<Student>(s => s.Id)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        
        builder.HasMany(s => s.StudentExams)
            .WithOne(se => se.Student)
            .HasForeignKey(se => se.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
