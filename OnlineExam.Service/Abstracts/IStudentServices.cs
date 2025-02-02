
using OnlineExam.Domain.Entities;
using OnlineExam.Domain.Entities.Identity;
using OnlineExam.Service.Enums;

namespace OnlineExam.Service.Abstracts
{
    public interface IStudentServices
    {
        public Task<(Student?, StudentResultEnum?)> UpdateStudentProfile(Student NewStudent, string Password);
        public Task<StudentResultEnum> DeleteStudentAccount(string StudentId, string Password);
        public Task<(IQueryable<StudentExam>?, StudentResultEnum?)> GetPassedStudentInExam(int ExamId);
        public Task<(IQueryable<StudentExam>?, StudentResultEnum?)> GetFailedStudentInExam(int ExamId);
        public Task<(IQueryable<Student>?, StudentResultEnum?)> GetStudentsBySchoolYear(int SchoolYear);
        public Task<(Student?, StudentResultEnum?)> GetStudentById(string StudentId);
        public Task<(List<StudentExam>?, StudentResultEnum)> StudentTakenExams(string StudentId);
        public Task<(List<Exam>?, StudentResultEnum)> StudentPendingExams(string StudentId);
        public Task<(List<Exam>?, StudentResultEnum?)> StudentAbsentExams(string StudentId);
    }
}
