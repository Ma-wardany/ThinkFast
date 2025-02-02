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
    public class StudentExamConfigs : IEntityTypeConfiguration<StudentExam>
    {
        public void Configure(EntityTypeBuilder<StudentExam> builder)
        {
            builder.ToTable("StudentExams");

            builder.HasKey(se => new {se.StudentId, se.ExamId});

            builder.Property(se => se.Grade)
                .IsRequired();
        }
    }
}
