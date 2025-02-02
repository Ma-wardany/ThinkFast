using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineExam.Domain.Entities;
using OnlineExam.Domain.Entities.Identity;
using OnlineExam.Infrastructure.Repository.Abstracts;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Service.Services
{
    public class StudentServices : IStudentServices
    {
        private readonly IExamRepository              examRepository;
        private readonly IStudentExamsRepository      studentExamsRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApplicationUserServices     applicationUserServices;
        private readonly IStudentRepository           studentRepository;

        public StudentServices(IExamRepository examRepository, 
            IStudentExamsRepository            studentExamsRepository, 
            UserManager<ApplicationUser>       userManager, 
            IApplicationUserServices           applicationUserServices,
            IStudentRepository                 studentRepository)
        {
            this.examRepository          = examRepository;
            this.studentExamsRepository  = studentExamsRepository;
            this.userManager             = userManager;
            this.applicationUserServices = applicationUserServices;
            this.studentRepository       = studentRepository;
        }
        

        public async Task<(Student?, StudentResultEnum?)> UpdateStudentProfile(Student NewStudent, string Password)
        {
            var ExistingStudent = await studentRepository.GetTableAsTracking()
                                       .Include(u => u.ApplicationUser)
                                       .SingleOrDefaultAsync(u => u.Id == NewStudent.Id);

            if (ExistingStudent == null)
                return (null, StudentResultEnum.STUDENT_NOT_EXIST);


            var IsCorrectPassword = await userManager.CheckPasswordAsync(ExistingStudent.ApplicationUser, Password);
            if (!IsCorrectPassword)
                return (null, StudentResultEnum.WRONG_PASSWORD);


            if(ExistingStudent.ApplicationUser.Email != NewStudent.ApplicationUser.Email)
                ExistingStudent.ApplicationUser.Email = NewStudent.ApplicationUser.Email;
            ExistingStudent.FullName   = NewStudent.FullName;
            ExistingStudent.Gender     = NewStudent.Gender;
            ExistingStudent.SchoolYear = NewStudent.SchoolYear;

            using var Trans = await studentExamsRepository.BeginTransactionAsync();
            try
            {
                await studentRepository.UpdateAsync(ExistingStudent);

                await Trans.CommitAsync();

                return (ExistingStudent, StudentResultEnum.UPDATED);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                await Trans.RollbackAsync();
                return(null, StudentResultEnum.FAILED);
            }
        }


        public async Task<StudentResultEnum> DeleteStudentAccount(string StudentId, string Password)
        {
            var student = await userManager.FindByIdAsync(StudentId);
            if(student == null)
                return StudentResultEnum.STUDENT_NOT_EXIST;


            var IsPasswordCorrect = await userManager.CheckPasswordAsync(student, Password);
            if(!IsPasswordCorrect)
                return StudentResultEnum.WRONG_PASSWORD;


            var result = await applicationUserServices.DeleteUser(StudentId);


            return result switch
            {
                ApplicationUserResultEnum.DELETED => StudentResultEnum.DELETED,
                ApplicationUserResultEnum.NOTFOUND_USER => StudentResultEnum.STUDENT_NOT_EXIST,
                _ or ApplicationUserResultEnum.FAILED => StudentResultEnum.FAILED,
            };
        }


        public Task<(IQueryable<Student>?, StudentResultEnum?)> GetStudentsBySchoolYear(int SchoolYear)
        {
            try
            {
                var students = studentRepository.GetTableNoTracking()
                                           .Where(s => s.SchoolYear == SchoolYear)
                                           .OrderBy(s => s.FullName);

                if (!students.Any())
                    return Task.FromResult<(IQueryable<Student>?, StudentResultEnum?)>((null, StudentResultEnum.EMPTY));

                return Task.FromResult<(IQueryable<Student>?, StudentResultEnum?)>((students, StudentResultEnum.SUCCESS));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return Task.FromResult<(IQueryable<Student>?, StudentResultEnum?)>((null, StudentResultEnum.FAILED));
            }

        }


        public async Task<(Student?, StudentResultEnum?)> GetStudentById(string StudentId)
        {
            try
            {
                var student = await studentRepository.GetTableNoTracking()
                                     .SingleOrDefaultAsync(s => s.Id == StudentId);

                if (student == null)
                    return (null, StudentResultEnum.STUDENT_NOT_EXIST);

                return (student, StudentResultEnum.SUCCESS);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return(null, StudentResultEnum.FAILED);
            }
        }


        public async Task<(List<StudentExam>?, StudentResultEnum)> StudentTakenExams(string StudentId)
        {
            var student = await studentRepository.GetTableNoTracking()
                                                 .SingleOrDefaultAsync(s => s.Id == StudentId);

            try
            {
                if (student == null)
                    return (null, StudentResultEnum.STUDENT_NOT_EXIST);

                var TakenExams = await studentExamsRepository.GetTableNoTracking()
                                                         .Include(se => se.Exam)
                                                         .ThenInclude(e => e.Subject)
                                                         .Where(se => se.StudentId == StudentId)
                                                         .ToListAsync();

                if (!TakenExams.Any())
                    return (null, StudentResultEnum.EMPTY);


                return (TakenExams, StudentResultEnum.SUCCESS);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return (null, StudentResultEnum.FAILED);
            }
            
        }


        public async Task<(List<Exam>?, StudentResultEnum)> StudentPendingExams(string StudentId)
        {
            var student = await studentRepository.GetTableNoTracking()
                                                 .SingleOrDefaultAsync(s => s.Id == StudentId);

            try
            {
                if (student == null)
                    return (null, StudentResultEnum.STUDENT_NOT_EXIST);

                var PendingExams = await examRepository.GetTableNoTracking()
                                                     .Include(e => e.Subject)
                                                     .Where(e => e.SchoolYear == student!.SchoolYear && e.Status == "pending")
                                                     .ToListAsync();

                if (!PendingExams.Any())
                    return (null, StudentResultEnum.EMPTY);


                return (PendingExams, StudentResultEnum.SUCCESS);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return (null , StudentResultEnum.FAILED);
            }
        }


        public async Task<(List<Exam>?, StudentResultEnum?)> StudentAbsentExams(string StudentId)
        {
            var student = await studentRepository.GetTableNoTracking()
                                     .SingleOrDefaultAsync(s => s.Id == StudentId);

            if (student == null)
                return (null, StudentResultEnum.STUDENT_NOT_EXIST);

            try
            {
                var AbsentExams = await examRepository.GetTableNoTracking()
                                                  .Include(e => e.Subject)
                                                  .Where(e => e.SchoolYear == student.SchoolYear
                                                               && e.Status == "ending"
                                                               && !e.StudentExams.Any(se => se.StudentId == StudentId))
                                                  .ToListAsync();

                if (!AbsentExams.Any())
                    return (null, StudentResultEnum.EMPTY);

                return (AbsentExams, StudentResultEnum.SUCCESS);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return (null, StudentResultEnum.FAILED);
            }
        }
        
        
        public async Task<(IQueryable<StudentExam>?, StudentResultEnum?)> GetPassedStudentInExam(int ExamId)
        {
            var result = await StatusOfStudentInExam(ExamId, PassedStatus: true);

            return result;
        }


        public async Task<(IQueryable<StudentExam>?, StudentResultEnum?)> GetFailedStudentInExam(int ExamId)
        {
            var result = await StatusOfStudentInExam(ExamId, PassedStatus: false);

            return result;
        }


        private async Task<(IQueryable<StudentExam>?, StudentResultEnum?)> StatusOfStudentInExam(int ExamId, bool PassedStatus)
        {
            var ExistExam = await examRepository.GetByIdAsync(ExamId);
            if (ExistExam == null)
                return (null, StudentResultEnum.EXAM_NOT_EXIST);


            try
            {
                var StudentsExams = studentExamsRepository.GetTableNoTracking()
                                        .Include(s => s.Student)
                                        .Include(s => s.Exam)
                                        .Where(er => er.ExamId == ExamId && PassedStatus ? er.Grade >= 50 : er.Grade < 50);

                if (!StudentsExams.Any())
                    return (null, StudentResultEnum.EMPTY);

                return (StudentsExams, StudentResultEnum.SUCCESS);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return (null, StudentResultEnum.FAILED);
            }
        }
    }
}
