using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Features.Students.Results
{
    public class UpdateStudentResultModel
    {
        public string? StudentId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public int SchoolYear { get; set; }
        public string? Gender { get; set; }
    }
}
