using AutoMapper;

namespace OnlineExam.Core.Mappings.ApplicationUserProfile
{
    public partial class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            RegisterUserMappings();
        }
    }
}
