using OnlineExam.Domain.Entities;
using OnlineExam.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Service.Abstracts
{
    public interface IEnrollmentServices
    {
        public Task<(Enrollment?, EnrollmentResultEnum?)> Enroll(Enrollment StudentEnrollment, string SubjectCode);
    }
}
