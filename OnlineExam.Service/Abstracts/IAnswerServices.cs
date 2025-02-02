using OnlineExam.Domain.Entities;
using OnlineExam.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Service.Abstracts
{
    public interface IAnswerServices
    {
        public Task<(IQueryable<Answer>?, AnswerResultEnum?)> SubmitAnswers(List<Answer> Answers);
    }
}
