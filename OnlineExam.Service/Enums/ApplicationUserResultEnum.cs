using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Service.Enums
{
    public enum ApplicationUserResultEnum
    {
        SUCCESS = 1,
        FAILED,
        DELETED,
        UPDATED,
        CREATED,
        EXIST_USER,
        INVALID_ROLE,
        CONFIRMED_SUCCESS,
        NOTFOUND_USER,
        WRONG_PASSWORD,
        UPDATED_PASSWORD
    }
}
