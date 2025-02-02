using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Questions.Results;

namespace OnlineExam.Core.Features.Questions.Queries.Models
{
    public class GetQuestionsForStudentsQuery : IRequest<Response<List<QuestionResultModel>>>
    {
        public string StudentId { get; set; }
        public int ExamId { get; set; }
    }
}
