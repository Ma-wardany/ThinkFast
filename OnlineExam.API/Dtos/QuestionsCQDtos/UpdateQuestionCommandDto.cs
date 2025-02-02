namespace OnlineExam.API.Dtos.QuestionsCQDtos
{
    public class UpdateQuestionCommandDto
    {
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public char CorrectAnswer { get; set; }
    }
}
