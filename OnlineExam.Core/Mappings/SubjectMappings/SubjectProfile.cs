using AutoMapper;
using OnlineExam.Core.Features.Subjects.Results;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.SubjectMappings
{
    public partial class SubjectProfile  :Profile
    {
        public SubjectProfile()
        {
            AddSubjectCommandProfile();
            UpdateSubjectCommandProfile();

            CreateMap<Subject, SubjectResultModel>()
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(
                    src => src.Instructor == null ?
                    "no instructor assigned yet!" :
                    src.Instructor.FirstName + " " + src.Instructor.LastName))

                .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(
                    src => src.Instructor == null ? "no instructor assigned yet!" : src.Instructor.Id))

                .ForMember(dest => dest.EnrollmentCount, opt => opt.MapFrom(src => src.Enrollments!.Count));
        }
    }
}
