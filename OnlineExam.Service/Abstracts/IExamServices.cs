using OnlineExam.Domain.Entities;
using OnlineExam.Infrastructure.Repository.Repositories;
using OnlineExam.Service.Enums;

namespace OnlineExam.Service.Abstracts
{
    public interface IExamServices
    {
        public Task<(Exam?, ExamResultEnum?)> AddExam(Exam? exam);

        public Task<(Exam?, ExamResultEnum?)> UpdateExam(Exam? NewExam);

        public Task<ExamResultEnum> DeleteExam(int ExamId, string InstructorId);

        public (IQueryable<Exam>?, ExamResultEnum?) GetExamsByInstructorId(string InstructorId);

        public Task<ExamResultEnum> EndExam(int ExamId);
    }
}