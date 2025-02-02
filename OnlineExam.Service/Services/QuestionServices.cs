using Microsoft.EntityFrameworkCore;
using OnlineExam.Domain.Entities;
using OnlineExam.Infrastructure.Repository.Abstracts;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Service.Services
{
    public class QuestionServices : IQuestionServices
    {
        private readonly IExamRepository examRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly IEnrollmentRepository enrollmentRepository;

        public QuestionServices(IExamRepository examRepository, IQuestionRepository questionRepository, IEnrollmentRepository enrollmentRepository)
        {
            this.examRepository     = examRepository;
            this.questionRepository = questionRepository;
            this.enrollmentRepository = enrollmentRepository;
        }

        public async Task<(Question?, QuestionResultEnum?)> AddQuestionToExam(Question question, string InstructorId)
        {
            var ExistExam = await examRepository.GetByIdAsync(question.ExamId);
            if (ExistExam == null)
                return (null, QuestionResultEnum.EXAM_NOT_EXIST);


            if (ExistExam.InstructorId != InstructorId)
                return (null, QuestionResultEnum.UNAUTHORIZED_ACCESS);

            using var Trans = await questionRepository.BeginTransactionAsync();
            try
            {
                await questionRepository.AddAsync(question);
                await Trans.CommitAsync();
                return (question, QuestionResultEnum.CREATED);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errors: " + ex.InnerException!.Message + "----------------");
                await Trans.RollbackAsync();
                return (null, QuestionResultEnum.FAILED);
            }
        }

        public async Task<(List<Question>?, QuestionResultEnum?)> AddQuestionsRangeToExam(List<Question> questions, string InstrctorId)
        {

            var ExamId = questions.First().ExamId;

            var ExistExam = await examRepository.GetByIdAsync(ExamId);
            if (ExistExam == null)
                return (null, QuestionResultEnum.EXAM_NOT_EXIST);

            if (ExistExam.InstructorId != InstrctorId)
                return (null, QuestionResultEnum.UNAUTHORIZED_ACCESS);

            using var Trans = await questionRepository.BeginTransactionAsync();
            try
            {
                await questionRepository.AddRangeAsync(questions);
                await Trans.CommitAsync();
                return (questions, QuestionResultEnum.CREATED);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors : {ex.InnerException!.Message}");
                await Trans.CommitAsync();
                return (null, QuestionResultEnum.FAILED);
            }
        }


        public async Task<(Question?, QuestionResultEnum?)> UpdateQuestion(Question NewQuestion, string InstructorId)
        {
            // Check if the question exists and belongs to the specified exam
            var ExistQuestion = await questionRepository.GetTableAsTracking()
                                                        .Include(q => q.Exam)
                                                        .SingleOrDefaultAsync(q => q.Id == NewQuestion.Id);
            if (ExistQuestion == null)
                return (null, QuestionResultEnum.NOT_FOUND_QUESTION);

            if (ExistQuestion.ExamId != NewQuestion.ExamId)
                return (null, QuestionResultEnum.MISMATCHED_EXAM_ID);

            // Check if the instructor is authorized for the exam
            if (ExistQuestion.Exam.InstructorId != InstructorId)
                return (null, QuestionResultEnum.UNAUTHORIZED_ACCESS);

            // Begin transaction for updates
            using var Trans = await questionRepository.BeginTransactionAsync();
            try
            {
                // Update the existing question's fields
                ExistQuestion.Content = NewQuestion.Content;
                ExistQuestion.OptionA = NewQuestion.OptionA;
                ExistQuestion.OptionB = NewQuestion.OptionB;
                ExistQuestion.OptionC = NewQuestion.OptionC;
                ExistQuestion.OptionD = NewQuestion.OptionD;
                ExistQuestion.CorrectAnswer = NewQuestion.CorrectAnswer;

                // Save changes
                await questionRepository.SaveChangesAsync();

                // Commit transaction
                await Trans.CommitAsync();

                return (ExistQuestion, QuestionResultEnum.UPDATED);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors : {ex.InnerException?.Message ?? ex.Message} -----------");
                await Trans.RollbackAsync();
                return (null, QuestionResultEnum.FAILED);
            }
        }


        public async Task<(List<Question>?, QuestionResultEnum?)> UpdateQuestionsRange(List<Question> NewQuestions, int ExamId, string InstructorId)
        {
            var QuestionsIDs = NewQuestions.Select(q => q.Id).ToList();

            var ExistQuestions = await questionRepository.GetTableAsTracking()
                                                    .Include(q => q.Exam)
                                                    .Where(q => QuestionsIDs.Contains(q.Id) && q.ExamId == ExamId)
                                                    .ToListAsync();
            if(ExistQuestions.Count != NewQuestions.Count)
                return (null, QuestionResultEnum.NOT_FOUND_QUESTION);



            if(ExistQuestions.First().Exam.InstructorId != InstructorId)
                return (null, QuestionResultEnum.UNAUTHORIZED_ACCESS);



            using var Trans = await questionRepository.BeginTransactionAsync();
            try
            {
                foreach (var existingQuestion in ExistQuestions)
                {
                    var question = NewQuestions.FirstOrDefault(q => q.Id == existingQuestion.Id);

                    existingQuestion.Content       = question.Content;
                    existingQuestion.OptionA       = question.OptionA;
                    existingQuestion.OptionB       = question.OptionB;
                    existingQuestion.OptionC       = question.OptionC;
                    existingQuestion.OptionD       = question.OptionD;
                    existingQuestion.CorrectAnswer = question.CorrectAnswer;
                }

                await questionRepository.UpdateRangeBulk(ExistQuestions);
                await questionRepository.CommitAsync();
                return (ExistQuestions, QuestionResultEnum.UPDATED);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                await questionRepository.RollBackAsync();
                return (null, QuestionResultEnum.FAILED);
            }
        }

        public async Task<QuestionResultEnum> DeleteQuestion(int QuestionId, int ExamId, string InstructorId)
        {
            var question = await questionRepository.GetTableAsTracking()
                                                   .Include(q => q.Exam)
                                                   .SingleOrDefaultAsync(q => q.Id == QuestionId);

            if (question == null)
                return QuestionResultEnum.NOT_FOUND_QUESTION;


            if (question.ExamId != ExamId)
                return QuestionResultEnum.EXAM_NOT_EXIST;


            if (question.Exam.InstructorId != InstructorId)
                return QuestionResultEnum.UNAUTHORIZED_ACCESS;


            using var Trans = await questionRepository.BeginTransactionAsync();
            try
            {
                await questionRepository.DeleteAsync(question);
                await questionRepository.CommitAsync();
                return QuestionResultEnum.DELETED;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException?.Message ?? ex.Message}");
                await questionRepository.RollBackAsync();
                return QuestionResultEnum.FAILED;
            }
        }


        public async Task<QuestionResultEnum> DeleteQuestionsRange(List<int> QuestionIDs, int ExamId, string InstructorId)
        {

            var ExistQuestions = questionRepository.GetTableAsTracking()
                                                         .Include(q => q.Exam)
                                                         .Where(q => QuestionIDs.Contains(q.Id) && q.ExamId == ExamId);
            if (ExistQuestions.Count() != QuestionIDs.Count())
                return QuestionResultEnum.NOT_FOUND_QUESTION;


            if(ExistQuestions.First().Exam.InstructorId != InstructorId)
                return QuestionResultEnum.UNAUTHORIZED_ACCESS;


            using var Trans = await questionRepository.BeginTransactionAsync();
            try
            {
                await ExistQuestions.ExecuteDeleteAsync();
                await questionRepository.CommitAsync();
                return QuestionResultEnum.DELETED;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                await questionRepository.RollBackAsync();
                return QuestionResultEnum.FAILED;
            }
        }

        public async Task<(IQueryable<Question>?, QuestionResultEnum?)> GetQuestionsByExamId(int ExamId, string InstructorId)
        {
            var ExistExam = await examRepository.GetByIdAsync(ExamId);
            if(ExistExam == null)
                return (null, QuestionResultEnum.EXAM_NOT_EXIST);

            if(ExistExam.InstructorId != InstructorId)
                return (null, QuestionResultEnum.UNAUTHORIZED_ACCESS);

            try
            {
                var questions = questionRepository.GetTableNoTracking()
                                                  .Include(q => q.Exam)
                                                  .Where(q => q.ExamId == ExamId);

                if(!questions.Any())
                    return (null, QuestionResultEnum.EMPTY);

                return (questions, QuestionResultEnum.SUCCESS);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return (null, QuestionResultEnum.FAILED);
            }
        }

        public async Task<(IQueryable<Question>?, QuestionResultEnum?)> GetQuestionForStudents(int ExamId, string StudentId)
        {
            // if exam is already exist
            var ExistingExam = await examRepository.GetTableNoTracking()
                                                   .Include(e => e.StudentExams)
                                                   .SingleOrDefaultAsync(e => e.Id == ExamId);                                 
            if (ExistingExam == null)
                return (null, QuestionResultEnum.EXAM_NOT_EXIST);


            // if student enrolled
            var IsEnrolled = await enrollmentRepository.GetTableNoTracking()
                                   .AnyAsync(en => en.StudentId == StudentId
                                                && en.SubjectId == ExistingExam.SubjectId);
            if(!IsEnrolled)
                return (null, QuestionResultEnum.STUDENT_NOT_ENROLLED);


            // if student finished this exam
            if (ExistingExam.StudentExams.Any(se => se.StudentId == StudentId))
                return (null, QuestionResultEnum.EXAM_DONE);


            try
            {
                var questions = questionRepository.GetTableNoTracking()
                                                        .Include(q => q.Exam)
                                                        .Where(q => q.ExamId == ExamId);


                if(!questions.Any())
                    return(null, QuestionResultEnum.EMPTY);


                return (questions, QuestionResultEnum.SUCCESS);                                        
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return (null, QuestionResultEnum.FAILED);
            }
        }
    }
}
