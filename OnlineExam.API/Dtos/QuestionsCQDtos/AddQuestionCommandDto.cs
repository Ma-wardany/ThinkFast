﻿namespace OnlineExam.API.Dtos.QuestionsCQDtos
{
    public class AddQuestionCommandDto
    {
        public int ExamId { get; set; }
        public string Content { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public char CorrectAnswer { get; set; }
    }
}
