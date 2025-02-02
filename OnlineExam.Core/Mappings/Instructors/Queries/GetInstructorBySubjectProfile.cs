using OnlineExam.Core.Features.Instructors.Results;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.Instructors
{
    public partial class InstructorMappingsProfile
    {
        public void GetInstructorBySubjectProfile()
        {
            CreateMap<Instructor, GetInstructorBySubjectResultModel>()
                .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => string.Concat(src.FirstName, " ", src.LastName)))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subjects.FirstOrDefault()));
        }

    }
}
