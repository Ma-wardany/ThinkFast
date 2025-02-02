using Microsoft.AspNetCore.Identity;
using OnlineExam.Domain.Entities;
using OnlineExam.Infrastructure.Repository.Repositories;
using OnlineExam.Service.Enums;
using OnlineExam.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Service.Abstracts
{
    public interface IInstructorServices
    {
        public Task<(Instructor?, InstructorResultEnum?)> UpdateInstructorProfile(Instructor NewInstructor, string Password);
        public Task<InstructorResultEnum> DeleteInstructorAccount(string InstructorId, string Password);
        public Task<(IQueryable<Instructor>?, InstructorResultEnum?)> GetAllInstructors();
        public Task<(IQueryable<Instructor>?, InstructorResultEnum?)> GetInstructorsBySchoolYear(int SchoolYear);
        public Task<(Instructor?, InstructorResultEnum?)> GetInstructorBySubject(int SubjectId);
    }
}
