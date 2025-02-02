using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineExam.Domain.Entities.Identity;



namespace OnlineExam.Infrastructure.Data.EntityConfigs
{
    public class ApplicationUserConfigs : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(a => a.Id);
        }
    }
}
