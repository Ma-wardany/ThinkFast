namespace OnlineExam.API.Dtos.StudentCQDto
{
    public class UpdateStudentProfileDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public int SchoolYear { get; set; }
        public string? Gender { get; set; }
        public string? Password { get; set; }
    }
}
