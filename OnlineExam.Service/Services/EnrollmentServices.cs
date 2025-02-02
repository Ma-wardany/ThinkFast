using Microsoft.EntityFrameworkCore;
using OnlineExam.Domain.Entities;
using OnlineExam.Infrastructure.Repository.Abstracts;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;
using System;
using System.Threading.Tasks;

namespace OnlineExam.Service.Services
{
    public class EnrollmentServices : IEnrollmentServices
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly IEnrollmentRepository enrollmentRepository;

        public EnrollmentServices(ISubjectRepository subjectRepository, IEnrollmentRepository enrollmentRepository)
        {
            this.subjectRepository = subjectRepository;
            this.enrollmentRepository = enrollmentRepository;
        }

        public async Task<(Enrollment?, EnrollmentResultEnum?)> Enroll(Enrollment studentEnrollment, string subjectCode)
        {
            var existEnrollment = await enrollmentRepository.GetTableAsTracking()
                                    .AnyAsync(e => e.StudentId == studentEnrollment.StudentId
                                                && e.SubjectId == studentEnrollment.SubjectId);
            if (existEnrollment)
                return (null, EnrollmentResultEnum.ALREADY_ENROLLED);


            var subject = await subjectRepository.GetTableNoTracking()
                                                 .SingleOrDefaultAsync(s => s.Id == studentEnrollment.SubjectId
                                                                       && s.Code == subjectCode);
            if (subject == null)
                return (null, EnrollmentResultEnum.SUBJECT_NOT_EXIST);


            if (subject.SchoolYear != studentEnrollment.SchoolYear)
                return (null, EnrollmentResultEnum.INVALID_SCHOOL_YEAR);


            using var transaction = await enrollmentRepository.BeginTransactionAsync();
            try
            {
                await enrollmentRepository.AddAsync(studentEnrollment);
                await enrollmentRepository.CommitAsync();

                var result = await enrollmentRepository.GetTableNoTracking()
                    .Include(e => e.Student)
                    .Include(e => e.Subject)
                    .ThenInclude(s => s.Instructor)
                    .FirstOrDefaultAsync(e => e.SubjectId == studentEnrollment.SubjectId
                                           && e.StudentId == studentEnrollment.StudentId);

                return (result, EnrollmentResultEnum.ENROLLED);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                await transaction.RollbackAsync();
                return (null, EnrollmentResultEnum.FAILED);
            }
        }
    }
}
