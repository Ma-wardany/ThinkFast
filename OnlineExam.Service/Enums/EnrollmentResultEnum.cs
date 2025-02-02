using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Service.Enums
{
    public enum EnrollmentResultEnum
    {
        SUBJECT_NOT_EXIST = 1,
        ALREADY_ENROLLED,
        FAILED,
        ENROLLED,
        INVALID_SCHOOL_YEAR,
    }
}
