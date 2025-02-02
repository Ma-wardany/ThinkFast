
using Microsoft.EntityFrameworkCore;
using OnlineExam.Domain.Entities;
using OnlineExam.Infrastructure.Repository.Abstracts;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Service.Services
{
    public class AnswerServices : IAnswerServices
    {
        private readonly IAnswerRepository answerRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly IStudentExamsRepository studentExamsRepository;

        public AnswerServices(IAnswerRepository answerRepository, 
                              IExamRepository examRepository,
                              IQuestionRepository questionRepository,
                              IStudentExamsRepository studentExamsRepository)
        {
            this.answerRepository       = answerRepository;
            this.questionRepository     = questionRepository;
            this.studentExamsRepository = studentExamsRepository;
        }


        public async Task<(IQueryable<Answer>?, AnswerResultEnum?)> SubmitAnswers(List<Answer> Answers)
        {
            var ExamId    = Answers.First().ExamId;
            var StudentId = Answers.First().StudentId;

            var IsSubmitted = await answerRepository.GetTableNoTracking()
                                               .FirstOrDefaultAsync(a => a.ExamId == ExamId && a.StudentId == StudentId);
            if (IsSubmitted != null)
                return (null, AnswerResultEnum.ALREADY_SUBMITTED);


            using var Trans = await answerRepository.BeginTransactionAsync();
            try
            {

                var questions = questionRepository.GetTableNoTracking()
                                      .Where(q => q.ExamId == ExamId);

                if (questions.Count() != Answers.Count)
                    return (null, AnswerResultEnum.INCOMLETE_ANSWER_LIST);


                var correctAnswers = Answers.Join(questions,
                                                  a => a.QuestionId,
                                                  q => q.Id,
                                                  (a, q) => new { a.SelectedAnswer, q.CorrectAnswer })
                                                  .Count(join => join.SelectedAnswer == join.CorrectAnswer);


                var grade = (int)(((double)correctAnswers / questions.Count()) * 100);
                Console.WriteLine($"Grade : {grade}----------------------------------");

                var StudentExam = new StudentExam
                {
                    StudentId = StudentId,
                    ExamId    = ExamId,
                    Grade     = grade,
                };


                await answerRepository.AddRangeAsync(Answers);
                await studentExamsRepository.AddAsync(StudentExam);


                var result = answerRepository.GetTableNoTracking()
                    .Include(a => a.Question)
                    .Where(a => a.StudentId == StudentId && a.ExamId == ExamId);


                await Trans.CommitAsync();
                return (result, AnswerResultEnum.SUBMITTED);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                await Trans.RollbackAsync();
                return (null, AnswerResultEnum.FAILED);
            }
        }
    }
}
