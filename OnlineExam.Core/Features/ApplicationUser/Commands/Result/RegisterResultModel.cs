
namespace OnlineExam.Core.Features.ApplicationUser.Commands.Result
{
    public class RegisterResultModel
    {
        public string UserName { get; set; }
        public string Email    { get; set; }
        public string RoleName { get; set; }
    }

    public class StudentRegisterResult : RegisterResultModel
    {
        public string FullName { get; set; }
        public string? Gender  { get; set; }
        public int SchoolYear  { get; set; }
    }
    public class InstructorRegisterResult : RegisterResultModel
    {
        public string FirstName { get; set; }
        public string LastName  { get; set; }
    }

}

