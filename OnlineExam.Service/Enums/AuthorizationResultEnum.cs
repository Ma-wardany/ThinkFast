using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Service.Enums
{
    public enum AuthorizationResultEnum
    {
        SUCCESS = 1,
        FAILED,
        EXIST_ROLE,
        CREATED,
        DELETED,
        UPDATED,
        INVALID_ROLE_NAME,
        NOTFOUND_ROLE,
        USED_ROLE,
        EMPTY
    }
}
