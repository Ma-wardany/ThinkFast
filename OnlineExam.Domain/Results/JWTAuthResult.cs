using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Domain.Results
{
    public class JWTAuthResult
    {
        public string UserId   { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public bool IsConfirmed { get; set; }
        public string AccessToken { get; set; }
        public RefreshTokenModel RefreshToken { get; set; }
    }

    public class RefreshTokenModel
    {
        public string RefreshTokenString { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
