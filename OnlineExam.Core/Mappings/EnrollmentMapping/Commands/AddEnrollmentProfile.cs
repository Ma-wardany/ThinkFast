using AutoMapper;
using OnlineExam.Core.Features.Enrollment.Commands.Models;
using OnlineExam.Core.Features.Enrollment.Results;
using OnlineExam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Mappings.EnrollmentMapping
{
    public partial class EnrollmentProfile
    {
        public void AddEnrollmentProfile()
        {
            CreateMap<EnrollmentCommand, Enrollment>()
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Subject, opt => opt.Ignore());
        }
    }
}
