using OnlineExam.Domain.Entities;
using OnlineExam.Service.Enums;

namespace OnlineExam.Service.Abstracts
{
    public interface IQuestionServices
    {
        public Task<(Question?, QuestionResultEnum?)> AddQuestionToExam(Question question, string InstructorId);
        public Task<(Question?, QuestionResultEnum?)> UpdateQuestion(Question NewQuestion, string InstructorId);
        public Task<(List<Question>?, QuestionResultEnum?)> AddQuestionsRangeToExam(List<Question> questions, string InstrctorId);
        public Task<(List<Question>?, QuestionResultEnum?)> UpdateQuestionsRange(List<Question> NewQuestions, int ExamId, string InstructorId);
        public Task<QuestionResultEnum> DeleteQuestion(int QuestionId, int ExamId, string InstructorId);
        public Task<QuestionResultEnum> DeleteQuestionsRange(List<int> QuestionIDs, int ExamId, string InstructorId);
        public Task<(IQueryable<Question>?, QuestionResultEnum?)> GetQuestionsByExamId(int ExamId, string InstructorId);
        public Task<(IQueryable<Question>?, QuestionResultEnum?)> GetQuestionForStudents(int ExamId, string StudentId);
    }
}