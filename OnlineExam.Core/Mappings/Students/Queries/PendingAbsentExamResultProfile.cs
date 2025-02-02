using OnlineExam.Core.Features.Students.Queries.Models;
using OnlineExam.Core.Features.Students.Results;
using OnlineExam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Mappings.Students
{
    public partial class StudentsMappingsProfile
    {
        public void PendingAbsentExamResultProfile()
        {
            CreateMap<Exam, PendingAbsentExamResultModel>()
                .ForMember(dest => dest.ExamName, opt => opt.MapFrom(src => src.ExamName))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.ExamDate, opt => opt.MapFrom(src => src.ExamDate));
        }
    }
}
