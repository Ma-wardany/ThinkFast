using OnlineExam.Core.Features.Instructors.DTOs;
using OnlineExam.Core.Features.Instructors.Results;
using OnlineExam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OnlineExam.Core.Mappings.Instructors
{
    public partial class InstructorMappingsProfile
    {
        public void GetInstructorResultProfile()
        {
            CreateMap<Instructor, GetInstructorResultModel>()
                .ForMember(dest => dest.InstructorId,    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.InstructorName,  opt => opt.MapFrom(src => string.Concat(src.FirstName, " ", src.LastName)))
                .ForMember(dest => dest.Subjects,        opt => opt.MapFrom(src => src.Subjects));

        }
    }
}
