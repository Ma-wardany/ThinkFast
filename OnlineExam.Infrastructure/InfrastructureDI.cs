using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OnlineExam.Domain.Entities.Identity;
using OnlineExam.Domain.Helpers;
using OnlineExam.Infrastructure.Data;
using OnlineExam.Infrastructure.Repository.Abstracts;
using OnlineExam.Infrastructure.Repository.Repositories;
using System.Text;


namespace OnlineExam.Infrastructure
{
    public static class InfrastructureDI
    {
        public static IServiceCollection InfrastructureDependencies(
            this IServiceCollection services,
            string ConnectionString,
            IConfiguration configuration)
        {



            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(ConnectionString);
            });


            services.AddIdentity<ApplicationUser, Role>(options =>
            {
                options.Password.RequireDigit     = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail   = true;

                options.Lockout.DefaultLockoutTimeSpan  = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers      = true;

            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(5);
            });

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<JwtSettings>(options =>
            {
                configuration.GetSection("JwtSettings").Bind(options);
                options.SignInKey = configuration["SignInKey"] ?? "";
            });


            var jwtSettings       = configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();
            jwtSettings.SignInKey = configuration.GetSection("SignInKey").Value!;
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme             = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken                 = true;
                options.RequireHttpsMetadata      = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidIssuer              = jwtSettings.Issuer,
                    ValidateAudience         = true,
                    ValidAudience            = jwtSettings.Audience,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey         = new SymmetricSecurityKey(
                                                   Encoding.UTF8.GetBytes(jwtSettings.SignInKey))
                };
            });



            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IInstructorRepository, InstructorRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<IStudentExamsRepository, StudentExamsRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            return services;
        }
    }
}
