using OnlineExam.Domain.Entities;
using OnlineExam.Infrastructure.Bases;
using OnlineExam.Infrastructure.Data;
using OnlineExam.Infrastructure.Repository.Abstracts;

namespace OnlineExam.Infrastructure.Repository.Repositories
{
    public class SubjectRepository : GenericaRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
