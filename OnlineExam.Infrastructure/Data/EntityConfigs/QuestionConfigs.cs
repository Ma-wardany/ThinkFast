using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineExam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Infrastructure.Data.EntityConfigs
{
    internal class QuestionConfigs : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");

            builder.HasKey(q => q.Id);

            builder.Property(q => q.Content)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(q => q.CorrectAnswer)
                .HasColumnType("VARCHAR(1)")
                .IsRequired();

            builder.Property(q => q.OptionA)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(q => q.OptionB)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(q => q.OptionC)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(q => q.OptionD)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();


            builder.HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
