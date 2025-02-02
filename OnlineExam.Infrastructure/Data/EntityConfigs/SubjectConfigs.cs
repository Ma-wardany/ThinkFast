using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineExam.Domain.Entities;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable("Subjects");

        builder.Property(s => s.Name)
            .HasColumnType("VARCHAR(50)")
            .IsRequired();

        builder.Property(s => s.Code)
            .HasColumnType("VARCHAR(50)")
            .IsRequired();

        builder.Property(s => s.SchoolYear)
            .IsRequired();

        builder.HasOne(s => s.Instructor)
            .WithMany(i => i.Subjects)
            .HasForeignKey(s => s.InstructorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Enrollments)
            .WithOne(e => e.Subject)
            .HasForeignKey(e => e.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Exams)
            .WithOne(e => e.Subject)
            .HasForeignKey(e => e.SubjectId)
            .OnDelete(DeleteBehavior.Restrict); // Avoid cycle here
    }
}
