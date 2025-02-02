using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineExam.Domain.Entities;
using OnlineExam.Domain.Entities.Identity;
using OnlineExam.Infrastructure.Repository.Abstracts;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Service.Services
{
    public class ExamServices : IExamServices
    {
        private readonly IExamRepository examRepository;
        private readonly ISubjectRepository subjectRepository;

        public ExamServices(
                            IExamRepository examRepository,
                            ISubjectRepository subjectRepository)
                            
                            
        {
            this.examRepository = examRepository;
            this.subjectRepository = subjectRepository;
        }

        public async Task<(Exam?, ExamResultEnum?)> AddExam(Exam? exam)
        {
            var subject = await subjectRepository.GetByIdAsync(exam!.SubjectId);
            if (subject == null)
                return (null, ExamResultEnum.NOTFOUND_SUBJECT);


            if(subject.InstructorId != exam.InstructorId)
                return (null, ExamResultEnum.WRONG_SUBJECT);


            var ExistExam = await examRepository.GetTableNoTracking()
                                                .FirstOrDefaultAsync(e => e.ExamCode == exam.ExamCode);
            if (ExistExam != null)
                return (null, ExamResultEnum.EXIST_EXAM);


            using var Trans = await examRepository.BeginTransactionAsync();
            try
            {
                await examRepository.AddAsync(exam);
                await Trans.CommitAsync();

                examRepository.GetTableAsTracking().Include(ex => ex.Instructor)
                              .FirstOrDefault(ex => ex.InstructorId == exam.InstructorId);
                return (exam, ExamResultEnum.CREATED);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                await Trans.RollbackAsync();
                return (null, ExamResultEnum.FAILED);
            }
        }

        public async Task<(Exam?, ExamResultEnum?)> UpdateExam(Exam? NewExam)
        {
            var ExistExam = await examRepository.GetTableAsTracking()
            .FirstOrDefaultAsync(e => e.Id == NewExam!.Id);
            if (ExistExam == null)
                return (null, ExamResultEnum.NOTFOUND_EXAM);


            var subject = await subjectRepository.GetByIdAsync(NewExam!.SubjectId);
            if (subject == null)
                return (null, ExamResultEnum.NOTFOUND_SUBJECT);


            if (NewExam.InstructorId != subject.InstructorId)
                return (null, ExamResultEnum.WRONG_SUBJECT);


            var IsExamCodeExist = await examRepository.GetTableNoTracking()
                                                      .AnyAsync(ex => ex.ExamCode == NewExam.ExamCode && ex.Id != ExistExam.Id);
            if (IsExamCodeExist)
                return (null, ExamResultEnum.EXIST_EXAM_CODE);


            using var Trans = await examRepository.BeginTransactionAsync();
            try
            {
                ExistExam.ExamName   = NewExam.ExamName;
                ExistExam.ExamDate   = NewExam.ExamDate;
                ExistExam.SchoolYear = NewExam.SchoolYear;
                ExistExam.SubjectId  = NewExam.SubjectId;
                ExistExam.ExamCode   = NewExam.ExamCode;

                await examRepository.UpdateAsync(ExistExam);
                await Trans.CommitAsync();
                return (ExistExam, ExamResultEnum.UPDATED);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                await Trans.RollbackAsync();
                return (null, ExamResultEnum.FAILED);
            }
        }

        public async Task<ExamResultEnum> DeleteExam(int ExamId, string InstructorId)
        {

            var ExistExam = await examRepository.GetTableAsTracking()
                                                .FirstOrDefaultAsync(e => e.Id == ExamId);
            if (ExistExam == null)
                return ExamResultEnum.NOTFOUND_EXAM;


            if (ExistExam.InstructorId != InstructorId)
                return ExamResultEnum.WRONG_SUBJECT;

            using var Trans = await examRepository.BeginTransactionAsync();
            try
            {
                await examRepository.DeleteAsync(ExistExam);
                await Trans.CommitAsync();
                return ExamResultEnum.DELETED;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                await Trans.RollbackAsync();
                return ExamResultEnum.FAILED;
            }
        }

        public (IQueryable<Exam>?, ExamResultEnum?) GetExamsByInstructorId(string InstructorId)
        {
            try
            {
                var exams = examRepository.GetTableNoTracking()
                          .Where(ex => ex.InstructorId == InstructorId);

                if (!exams.Any())
                    return (null, ExamResultEnum.EMPTY);

                return (exams, ExamResultEnum.SUCCESS);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException!.Message);
                return (null, ExamResultEnum.FAILED);
            }
        }

        public async Task<ExamResultEnum> EndExam(int ExamId)
        {
            var ExistExam = await examRepository.GetByIdAsync(ExamId);
            if (ExistExam == null)
                return ExamResultEnum.NOTFOUND_EXAM;

            if (ExistExam.Status == "ending")
                return ExamResultEnum.ARLEADY_ENDED;

            try
            {
                ExistExam.Status = "ending";
                await examRepository.UpdateAsync(ExistExam);
                return ExamResultEnum.UPDATED;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return ExamResultEnum.FAILED;
            }

        }
    }
}
