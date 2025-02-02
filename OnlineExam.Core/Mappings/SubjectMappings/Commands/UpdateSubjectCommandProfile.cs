using AutoMapper;
using OnlineExam.Core.Features.Subjects.Commands.Models;
using OnlineExam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Mappings.SubjectMappings
{
    public partial class SubjectProfile
    {
        public void UpdateSubjectCommandProfile()
        {
            CreateMap<UpdateSubjectCommand, Subject>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SubjectId));
        
        }
    }
}
