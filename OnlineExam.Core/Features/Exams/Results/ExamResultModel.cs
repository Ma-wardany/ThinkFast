using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineExam.Core.Features.Exams.Results
{
    public class ExamResultModel
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public string ExamCode { get; set; }
        public string SubjectName { get; set; }
        public string InstructorName { get; set; }
        public DateTime ExamDate { get; set; }
        public string Status { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int QuestionsCount { get; set; }
    }
}