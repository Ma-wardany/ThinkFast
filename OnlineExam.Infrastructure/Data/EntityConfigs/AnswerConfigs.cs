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
    public class AnswerConfigs : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers");

            builder.HasKey(x => x.AnswerId);

            builder.Property(a => a.SelectedAnswer)
                .HasColumnType("VARCHAR(1)")
                .IsRequired();



        }
    }
}
