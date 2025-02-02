using OnlineExam.Domain.Entities;
using OnlineExam.Infrastructure.Bases;
using OnlineExam.Infrastructure.Data;
using OnlineExam.Infrastructure.Repository.Abstracts;

namespace OnlineExam.Infrastructure.Repository.Repositories
{
    public class ExamRepository : GenericaRepository<Exam>, IExamRepository
    {
        public ExamRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
