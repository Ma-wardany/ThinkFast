

using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Exams.Results;

namespace OnlineExam.Core.Features.Exams.Commands.Models
{
    public class UpdateExamCommand : IRequest<Response<ExamResultModel>>
    {
        public int? Id              { get; set; }
        public string? InstructorId { get; set; }
        public string? ExamName     { get; set; }
        public DateTime? ExamDate   { get; set; }
        public int? SchoolYear      { get; set; }
        public int? SubjectId       { get; set; }
        public string? ExamCode     { get; set; }
    }
}