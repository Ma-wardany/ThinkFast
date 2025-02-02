
using OnlineExam.Domain.Entities;
using OnlineExam.Infrastructure.Bases;
using OnlineExam.Infrastructure.Data;
using OnlineExam.Infrastructure.Repository.Abstracts;

namespace OnlineExam.Infrastructure.Repository.Repositories
{
    public class AnswerRepository : GenericaRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
