using AutoMapper;
using OnlineExam.Core.Features.Exams.Commands.Models;
using OnlineExam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Mappings.ExamMapping
{
    public partial class ExamProfile
    {
        public void UpdateExamCommandProfile()
        {
            CreateMap<UpdateExamCommand, Exam>().ReverseMap();
        }
    }
}
