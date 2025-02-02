using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Exams.Results;
using OnlineExam.Core.Features.Questions.DTOs;

namespace OnlineExam.Core.Features.Exams.Commands.Models
{
    public class AddExamCommand : IRequest<Response<ExamResultModel>>
    {
        public string InstructorId            { get; set; }
        public int SubjectId                  { get; set; }
        public string ExamName                { get; set; }
        public string ExamCode                { get; set; }
        public DateTime ExamDate              { get; set; }
        public int SchoolYear                 { get; set; }
    }
}
