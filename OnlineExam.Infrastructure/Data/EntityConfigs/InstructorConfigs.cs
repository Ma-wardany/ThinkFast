using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineExam.Domain.Entities;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.ToTable("Instructors");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FirstName)
            .HasColumnType("VARCHAR(30)")
            .IsRequired();

        builder.Property(i => i.LastName)
            .HasColumnType("VARCHAR(30)")
            .IsRequired();

        builder.HasMany(i => i.Subjects)
            .WithOne(s => s.Instructor)
            .HasForeignKey(s => s.InstructorId)
            .OnDelete(DeleteBehavior.Cascade); // Retain cascade for Instructor-Subject

        builder.HasOne(i => i.ApplicationUser)
            .WithOne(a => a.Instructor)
            .HasForeignKey<Instructor>(i => i.Id)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
