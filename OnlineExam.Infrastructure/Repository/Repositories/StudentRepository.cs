using OnlineExam.Domain.Entities;
using OnlineExam.Infrastructure.Bases;
using OnlineExam.Infrastructure.Data;
using OnlineExam.Infrastructure.Repository.Abstracts;

namespace OnlineExam.Infrastructure.Repository.Repositories
{
    public class StudentRepository : GenericaRepository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context) : base(context) { }
    }
}
