using OnlineExam.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, Role, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students           { get; set; }
        public DbSet<Instructor> Instructors     { get; set; }
        public DbSet<Exam> Exams                 { get; set; }
        public DbSet<Question> Questions         { get; set; }
        public DbSet<Subject> Subjects           { get; set; }
        public DbSet<StudentExam> StudentExams   { get; set; }
        public DbSet<Answer> Answers             { get; set; }
        public DbSet<Enrollment> Enrollments     { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}