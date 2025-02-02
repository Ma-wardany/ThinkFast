using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Service.Enums
{
    public enum ExamResultEnum
    {
        EXIST_EXAM = 1,
        UNAUTHORIZED,
        NOTFOUND_SUBJECT,
        CREATED,
        FAILED,
        WRONG_SUBJECT,
        NOTFOUND_EXAM,
        UPDATED,
        EXIST_EXAM_CODE,
        DELETED,
        SUCCESS,
        EMPTY,
        ARLEADY_ENDED
    }
}
