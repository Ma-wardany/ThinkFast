
namespace OnlineExam.Domain.Entities.Identity
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshTokenString { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsRevoked { get; set; }

    }
}
