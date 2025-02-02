using AutoMapper;
using OnlineExam.Core.Features.Subjects.Commands.Models;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.SubjectMappings
{
    public partial class SubjectProfile
    {
        public void AddSubjectCommandProfile()
        {
            CreateMap<AddSubjectCommand, Subject>();
        }
    }
}
