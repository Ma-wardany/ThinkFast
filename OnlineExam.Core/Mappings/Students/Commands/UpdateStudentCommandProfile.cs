using AutoMapper;
using OnlineExam.Core.Features.Students.Commands.Models;
using OnlineExam.Core.Features.Students.Results;
using OnlineExam.Domain.Entities;
using OnlineExam.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Mappings.Students
{
    public partial class StudentsMappingsProfile
    {
        public void UpdateStudentCommandProfile()
        {
            CreateMap<UpdateStudentCommand, Student>()
                .ForMember(dest => dest.Id,                  opt => opt.MapFrom(src => src.StudentId))
                .ForPath(dest => dest.ApplicationUser.Email, opt => opt.MapFrom(src => src.Email));


            CreateMap<Student, UpdateStudentResultModel>()
                .ForMember(dest => dest.StudentId,  opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email,      opt => opt.MapFrom(src => src.ApplicationUser.Email))
                .ForMember(dest => dest.FullName,   opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Gender,     opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.SchoolYear, opt => opt.MapFrom(src => src.SchoolYear));


        }
    }
}
