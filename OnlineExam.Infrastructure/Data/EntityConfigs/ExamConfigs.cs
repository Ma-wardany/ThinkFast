using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineExam.Domain.Entities;

public class ExamConfiguration : IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder.ToTable("Exams");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.ExamName)
            .HasColumnType("VARCHAR(50)")
            .IsRequired();

        builder.Property(e => e.ExamDate)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(e => e.SchoolYear)
            .IsRequired();

        builder.HasOne(e => e.Instructor)
            .WithMany(i => i.Exams)
            .HasForeignKey(e => e.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Subject)
            .WithMany(s => s.Exams)
            .HasForeignKey(e => e.SubjectId)
            .OnDelete(DeleteBehavior.Restrict); // Avoid cycle here

        builder.HasMany(e => e.Questions)
            .WithOne(q => q.Exam)
            .HasForeignKey(e => e.ExamId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.StudentExams)
            .WithOne(se => se.Exam)
            .HasForeignKey(se => se.ExamId)
            .OnDelete(DeleteBehavior.Restrict); // No cascading delete here
    }
}
