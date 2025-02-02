
using Microsoft.EntityFrameworkCore;
using OnlineExam.Domain.Entities;
using OnlineExam.Infrastructure.Repository.Abstracts;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Service.Services
{
    public class SubjectServices : ISubjectServices
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly IInstructorRepository instructorRepository;
        private readonly IExamRepository examRepository;

        public SubjectServices(ISubjectRepository subjectRepository, IInstructorRepository instructorRepository, IExamRepository examRepository)
        {
            this.subjectRepository    = subjectRepository;
            this.instructorRepository = instructorRepository;
            this.examRepository = examRepository;
        }


        public async Task<(Subject?, SubjectResultEnum?)> AddNewSubject(Subject subject)
        {
            var IsSubjectExsit = await subjectRepository.GetTableNoTracking()
                                    .AnyAsync(s => s.Code == subject.Code);

            if (IsSubjectExsit)
                return (null, SubjectResultEnum.ALREADY_EXIST);


            if(subject.InstructorId != null)
            {
                var IsInstructorExist = await instructorRepository.GetTableNoTracking()
                                                           .AnyAsync(i => i.Id == subject.InstructorId);
                if (!IsInstructorExist)
                    return (null, SubjectResultEnum.NOT_FOUND_INSTRUCTOR);
            }


            using var Trans = await subjectRepository.BeginTransactionAsync();
            try
            {
                await subjectRepository.AddAsync(subject);
                await Trans.CommitAsync();

                var AddedSubject = await subjectRepository.GetTableNoTracking()
                                         .Include(s => s.Instructor)
                                         .FirstOrDefaultAsync(s => s.Id == subject.Id);

                return (AddedSubject, SubjectResultEnum.CREATED);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                await Trans.RollbackAsync();
                return (null, SubjectResultEnum.FAILED);
            }
        }


        public async Task<(Subject?, SubjectResultEnum?)> UpdateSubject(Subject NewSubject)
        {
            var ExistSubject = await subjectRepository.GetTableAsTracking()
                                                      .Include(s => s.Instructor)
                                                      .Include(s => s.Enrollments)
                                                      .FirstOrDefaultAsync(s => s.Id == NewSubject.Id);
            if (ExistSubject == null)
                return (null, SubjectResultEnum.NOT_EXIST);



            if (NewSubject.InstructorId != null && NewSubject.InstructorId != ExistSubject.InstructorId)
            {
                var IsInstructorExist = await instructorRepository.GetTableNoTracking()
                                              .AnyAsync(i => i.Id == NewSubject.InstructorId);
                if (!IsInstructorExist)
                    return (null, SubjectResultEnum.NOT_FOUND_INSTRUCTOR);
            }


            // update values
            ExistSubject.Id           = NewSubject.Id;
            ExistSubject.Name         = NewSubject.Name;
            ExistSubject.InstructorId = NewSubject.InstructorId;
            ExistSubject.Code         = NewSubject.Code;
            ExistSubject.SchoolYear   = NewSubject.SchoolYear;


            using var Trans = await subjectRepository.BeginTransactionAsync();
            try
            {
                await subjectRepository.UpdateAsync(ExistSubject);
                await Trans.CommitAsync();

                return (ExistSubject, SubjectResultEnum.UPDATED);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                await Trans.RollbackAsync();
                return (null, SubjectResultEnum.FAILED);
            }
        }


        public async Task<SubjectResultEnum> DeleteSubject(int SubjectId)
        {
            var subject = await subjectRepository.GetByIdAsync(SubjectId);
            if(subject == null)
                return SubjectResultEnum.NOT_EXIST;


            // Check if there are any Exams referencing this subject
            var exams = examRepository.GetTableNoTracking().Where(e => e.SubjectId == SubjectId);
            if (exams.Any())
            {
                return SubjectResultEnum.CANNOT_DELETE_HAS_EXAMS;
            }

            using var Trans = await subjectRepository.BeginTransactionAsync();
            try
            {
                await subjectRepository.DeleteAsync(subject);
                await Trans.CommitAsync();

                return SubjectResultEnum.DELETED;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                await Trans.RollbackAsync();
                return SubjectResultEnum.FAILED;
            }
        }


        public (IQueryable<Subject>?, SubjectResultEnum?) GetAllSubjects()
        {
            var subjects = subjectRepository.GetTableNoTracking()
                                            .Include(s => s.Instructor)
                                            .Include(s => s.Enrollments)
                                            .Select(s => new Subject
                                            {
                                                Id = s.Id,
                                                Name = s.Name,
                                                Code = s.Code,
                                                SchoolYear = s.SchoolYear,
                                                Enrollments = s.Enrollments,
                                                Instructor = s.Instructor
                                            });

            if (subjects == null)
                return (null, SubjectResultEnum.EMPTY);

            return (subjects, SubjectResultEnum.SUCCESS);
        }


        public (IQueryable<Subject>?, SubjectResultEnum?) GetSubjectsWithoutInstructors()
        {
            var subjects = subjectRepository.GetTableNoTracking()
                                            .Where(s => s.InstructorId == null);


            if (subjects == null)
                return (null, SubjectResultEnum.EMPTY);

            return (subjects, SubjectResultEnum.SUCCESS);
        }
    }
}