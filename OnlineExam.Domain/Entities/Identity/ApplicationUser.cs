using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace OnlineExam.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string RoleName                          { get; set; }
        public virtual Student? Student                 { get; set; }
        public virtual Instructor? Instructor           { get; set; }
        public virtual List<RefreshToken> RefreshTokens { get; set; }
    }
}
