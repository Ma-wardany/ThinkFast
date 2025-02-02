using OnlineExam.Domain.Entities;
using OnlineExam.Infrastructure.Bases;
using OnlineExam.Infrastructure.Data;
using OnlineExam.Infrastructure.Repository.Abstracts;

namespace OnlineExam.Infrastructure.Repository.Repositories
{
    public class EnrollmentRepository : GenericaRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
