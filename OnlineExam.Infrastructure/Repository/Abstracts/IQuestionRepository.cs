using OnlineExam.Domain.Entities;
using OnlineExam.Infrastructure.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Infrastructure.Repository.Abstracts
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
    }
}
