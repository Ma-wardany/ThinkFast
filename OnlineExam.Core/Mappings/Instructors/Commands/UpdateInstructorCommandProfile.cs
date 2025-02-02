

using OnlineExam.Core.Features.Instructors.Commands.Models;
using OnlineExam.Core.Features.Instructors.Results;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.Instructors
{
    public partial class InstructorMappingsProfile
    {

        public void UpdateInstructorCommandProfile()
        {
            CreateMap<UpdateInstructorProfileCommand, Instructor>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InstructorId))
                .ForPath(dest => dest.ApplicationUser.Email, opt => opt.MapFrom(src => src.Email));


            CreateMap<Instructor, UpdateInstructorProfileResultModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.Concat(src.FirstName, " ", src.LastName)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email));
        }
    }
}
