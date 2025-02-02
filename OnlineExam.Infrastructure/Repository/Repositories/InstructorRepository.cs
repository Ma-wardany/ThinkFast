using OnlineExam.Domain.Entities;
using OnlineExam.Infrastructure.Bases;
using OnlineExam.Infrastructure.Data;
using OnlineExam.Infrastructure.Repository.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Infrastructure.Repository.Repositories
{
    public class InstructorRepository : GenericaRepository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
