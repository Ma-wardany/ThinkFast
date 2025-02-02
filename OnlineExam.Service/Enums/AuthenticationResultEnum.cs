using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Service.Enums
{
    public enum AuthenticationResultEnum
    {
        INCORRECT_EMAIL_PASSWORD = 1,
        FAILED,
        SUCCESS,
        TRY_LATER,
        NOT_CONFIRMED,
        UNAUTHORIZED,
        INVALID_ACCESS_TOKEN,
        INVALID_REFRESH_TOKEN,
        NOTFOUND_USER,
        SENDING_ERROR,
        INVALID_OTP,
        VERIFIED_OTP,
        INVALID_RESET_TOKEN,
        PASSWORD_RESET
    }
}
