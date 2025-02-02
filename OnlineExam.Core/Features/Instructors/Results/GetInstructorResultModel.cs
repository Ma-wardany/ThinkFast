using OnlineExam.Core.Features.Instructors.DTOs;
using OnlineExam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Features.Instructors.Results
{
    public class GetInstructorResultModel
    {
        public string InstructorId { get; set; }
        public string InstructorName { get; set; }
        public List<InstructorSubjectDto> Subjects { get; set; }
    }
}
