using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineExam.Domain.Entities;
using OnlineExam.Domain.Entities.Identity;
using OnlineExam.Infrastructure.Repository.Abstracts;
using OnlineExam.Infrastructure.Repository.Repositories;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Service.Services
{
    public class InstructorServices : IInstructorServices
    {
        private readonly IInstructorRepository instructorRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApplicationUserServices applicationUserServices;
        private readonly ISubjectRepository subjectRepository;

        public InstructorServices(IInstructorRepository instructorRepository, 
            UserManager<ApplicationUser> userManager, 
            IApplicationUserServices applicationUserServices,
            ISubjectRepository subjectRepository)
        {
            this.instructorRepository = instructorRepository;
            this.userManager          = userManager;
            this.applicationUserServices = applicationUserServices;
            this.subjectRepository = subjectRepository;
        }

        public async Task<(Instructor?, InstructorResultEnum?)> UpdateInstructorProfile(Instructor NewInstructor, string Password)
        {
            var ExistingInstructor = await instructorRepository.GetTableAsTracking()
                                                            .Include(i => i.ApplicationUser)
                                                            .SingleOrDefaultAsync(i => i.Id == NewInstructor.Id);

            if (ExistingInstructor == null)
                return (null, InstructorResultEnum.NOTFOUND_INSTRUCTOR);


            var IsCorrectPassword = await userManager.CheckPasswordAsync(ExistingInstructor.ApplicationUser, Password);
            if (!IsCorrectPassword)
                return (null, InstructorResultEnum.WRONG_PASSWORD);


            if(ExistingInstructor.ApplicationUser.Email != NewInstructor.ApplicationUser.Email)
                ExistingInstructor.ApplicationUser.Email = NewInstructor.ApplicationUser.Email;
            ExistingInstructor.FirstName = NewInstructor.FirstName;
            ExistingInstructor.LastName  = NewInstructor.LastName;
            
            using var Trans = await instructorRepository.BeginTransactionAsync();
            try
            {
                await instructorRepository.UpdateAsync(ExistingInstructor);
                await Trans.CommitAsync();
                return (ExistingInstructor, InstructorResultEnum.UPDATED);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                await Trans.RollbackAsync();
                return (null, InstructorResultEnum.FAILED);
            }
        }

        public async Task<InstructorResultEnum> DeleteInstructorAccount(string InstructorId, string Password)
        {
            var instructor = await userManager.FindByIdAsync(InstructorId);
            if (instructor == null)
                return InstructorResultEnum.NOTFOUND_INSTRUCTOR;


            var IsPasswordCorrect = await userManager.CheckPasswordAsync(instructor, Password);
            if (!IsPasswordCorrect)
                return InstructorResultEnum.WRONG_PASSWORD;


            var result = await applicationUserServices.DeleteUser(InstructorId);


            return result switch
            {
                ApplicationUserResultEnum.DELETED       => InstructorResultEnum.DELETED,
                ApplicationUserResultEnum.NOTFOUND_USER => InstructorResultEnum.NOTFOUND_INSTRUCTOR,
                _ or ApplicationUserResultEnum.FAILED   => InstructorResultEnum.FAILED,
            };

        }

        public Task<(IQueryable<Instructor>?, InstructorResultEnum?)> GetAllInstructors()
        {
            try
            {
                var instructors = instructorRepository.GetTableNoTracking().Include(i => i.Subjects);

                if (!instructors.Any())
                    return Task.FromResult<(IQueryable<Instructor>?, InstructorResultEnum?)>((null, InstructorResultEnum.EMPTY));

                return Task.FromResult<(IQueryable<Instructor>?, InstructorResultEnum?)>((instructors, InstructorResultEnum.SUCCESS));
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return Task.FromResult<(IQueryable<Instructor>?, InstructorResultEnum?)>((null, InstructorResultEnum.FAILED));
            }
        }

        public Task<(IQueryable<Instructor>?, InstructorResultEnum?)> GetInstructorsBySchoolYear(int SchoolYear)
        {
            try
            {
                var instructors = instructorRepository.GetTableNoTracking()
                                               .Include(i => i.Subjects)
                                               .Where(i => i.Subjects.Any(s => s.SchoolYear ==  s.SchoolYear));

                if (!instructors.Any())
                    return Task.FromResult<(IQueryable<Instructor>?, InstructorResultEnum?)>((null, InstructorResultEnum.EMPTY));

                return Task.FromResult<(IQueryable<Instructor>?, InstructorResultEnum?)>((instructors, InstructorResultEnum.SUCCESS));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return Task.FromResult<(IQueryable<Instructor>?, InstructorResultEnum?)>((null, InstructorResultEnum.FAILED));
            }
        }


        public async Task<(Instructor?, InstructorResultEnum?)> GetInstructorBySubject(int SubjectId)
        {
            var subject = await subjectRepository.GetByIdAsync(SubjectId);
            if (subject == null)
                return (null, InstructorResultEnum.NOTFOUND_SUBJECT);

            try
            {
                var instructor = await instructorRepository.GetTableNoTracking()
                                       .Include(i => i.Subjects)
                                       .SingleOrDefaultAsync(i => i.Id == subject.InstructorId);

                return (instructor, InstructorResultEnum.SUCCESS);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return (null, InstructorResultEnum.FAILED);
            }
        }
    }
}
