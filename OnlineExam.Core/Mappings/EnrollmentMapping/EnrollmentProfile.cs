using AutoMapper;
using OnlineExam.Core.Features.Enrollment.Results;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.EnrollmentMapping
{
    public partial class EnrollmentProfile : Profile
    {
        public EnrollmentProfile()
        {
            AddEnrollmentProfile();

            CreateMap<Enrollment, EnrollmentResultModel>()
                .ForMember(dest => dest.StudentName,    opt => opt.MapFrom(src => src.Student.FullName))
                .ForMember(dest => dest.SubjectName,    opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.SchoolYear,     opt => opt.MapFrom(src => src.Subject.SchoolYear))
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Subject.Instructor.FirstName + " " + src.Subject.Instructor.LastName))
                .ReverseMap();
        }
    }
}
