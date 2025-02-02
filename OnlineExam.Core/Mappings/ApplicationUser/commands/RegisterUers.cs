using OnlineExam.Core.Features.ApplicationUser.Commands.Models;
using OnlineExam.Core.Features.ApplicationUser.Commands.Result;
using OnlineExam.Domain.Entities.Identity;

namespace OnlineExam.Core.Mappings.ApplicationUserProfile
{
    public partial class ApplicationUserProfile
    {
        public void RegisterUserMappings()
        {
            CreateMap<RegisterStudentCommand, ApplicationUser>()     
                .ForMember(dest => dest.RoleName,          opt => opt.MapFrom(src => src.RoleName))
                .ForPath(dest => dest.Student!.FullName,   opt => opt.MapFrom(src => src.FullName))
                .ForPath(dest => dest.Student!.Gender,     opt => opt.MapFrom(src => src.Gender))
                .ForPath(dest => dest.Student!.SchoolYear, opt => opt.MapFrom(src => src.SchoolYear))
                .ReverseMap();

            CreateMap<RegisterInstructorCommand, ApplicationUser>()
                .ForMember(dest => dest.RoleName,            opt => opt.MapFrom(src => src.RoleName))
                .ForPath(dest => dest.Instructor!.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForPath(dest => dest.Instructor!.LastName,  opt => opt.MapFrom(src => src.LastName))
                .ReverseMap();

            CreateMap<ApplicationUser, StudentRegisterResult>()
                .ForMember(dest => dest.RoleName,   opt => opt.MapFrom(src => src.RoleName))
                .ForMember(dest => dest.UserName,   opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email,      opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName,   opt => opt.MapFrom(src => src.Student!.FullName))
                .ForMember(dest => dest.Gender,     opt => opt.MapFrom(src => src.Student!.Gender))
                .ForMember(dest => dest.SchoolYear, opt => opt.MapFrom(src => src.Student!.SchoolYear))
                .ReverseMap();


            CreateMap<ApplicationUser, InstructorRegisterResult>()
                .ForMember(dest => dest.RoleName,  opt => opt.MapFrom(src => src.RoleName))
                .ForMember(dest => dest.UserName,  opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email,     opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Instructor!.FirstName))
                .ForMember(dest => dest.LastName,  opt => opt.MapFrom(src => src.Instructor!.LastName))
                .ReverseMap();

        }
    }
}
