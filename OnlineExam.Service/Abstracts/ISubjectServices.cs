
using OnlineExam.Domain.Entities;
using OnlineExam.Service.Enums;

namespace OnlineExam.Service.Abstracts
{
    public interface ISubjectServices
    {
        public Task<(Subject?, SubjectResultEnum?)> AddNewSubject(Subject subject);
        public Task<(Subject?, SubjectResultEnum?)> UpdateSubject(Subject NewSubject);
        public Task<SubjectResultEnum> DeleteSubject(int SubjectId);
        public (IQueryable<Subject>?, SubjectResultEnum?) GetAllSubjects();
        public (IQueryable<Subject>?, SubjectResultEnum?) GetSubjectsWithoutInstructors();
    }
}
