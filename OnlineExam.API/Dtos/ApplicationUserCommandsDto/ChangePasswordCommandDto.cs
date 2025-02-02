namespace OnlineExam.API.Dtos.ApplicationUserCommandsDto
{
    public class ChangePasswordCommandDto
    {
        public string CurrentPassword { get; set; }
        public string NewPssword { get; set; }
    }
}
