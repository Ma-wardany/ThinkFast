
using OnlineExam.Domain.Entities;
using OnlineExam.Infrastructure.Bases;
using OnlineExam.Infrastructure.Data;
using OnlineExam.Infrastructure.Repository.Abstracts;

namespace OnlineExam.Infrastructure.Repository.Repositories
{
    public class StudentExamsRepository : GenericaRepository<StudentExam>, IStudentExamsRepository
    {
        public StudentExamsRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
