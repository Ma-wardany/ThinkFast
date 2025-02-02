using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Features.Students.Results
{
    public class StudentExamsResultModel
    {
        public string ExamName { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string Gender { get; set; }
        public int SchoolYear { get; set; }
        public int Grade { get; set; }
    }
}
