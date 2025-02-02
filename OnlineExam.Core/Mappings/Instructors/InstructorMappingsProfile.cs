using AutoMapper;
using OnlineExam.Core.Features.Instructors.DTOs;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.Instructors
{
    public partial class InstructorMappingsProfile : Profile
    {
        public InstructorMappingsProfile()
        {
            UpdateInstructorCommandProfile();
            GetInstructorResultProfile();
            GetInstructorBySubjectProfile();

            CreateMap<Subject, InstructorSubjectDto>()
                .ForMember(dest => dest.SubjectId,   opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SchoolYear,  opt => opt.MapFrom(src => src.SchoolYear));
        }
    }
}
