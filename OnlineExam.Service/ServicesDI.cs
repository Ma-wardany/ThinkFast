using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Services;
using OnlineExam.Service.BackgroundServices;


namespace OnlineExam.Service
{
    public static class ServicesDI
    {
        public static IServiceCollection ServicesDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationServices, AuthorizationServices>();
            services.AddScoped<IApplicationUserServices, ApplicationUserServices>();
            services.AddScoped<IEmailServices, EmailService>();
            services.AddScoped<IEmailNotificationServices, EmailNotificationServices>();
            services.AddScoped<IExamServices, ExamServices>();
            services.AddScoped<IQuestionServices, QuestionServices>();
            services.AddScoped<IEnrollmentServices, EnrollmentServices>();
            services.AddScoped<IAnswerServices, AnswerServices>();
            services.AddScoped<IStudentServices, StudentServices>();
            services.AddScoped<ISubjectServices, SubjectServices>();
            services.AddScoped<IAuthenticationsServices, AuthenticationsServices>();
            services.AddScoped<IInstructorServices, InstructorServices>();



            services.AddHostedService<DeleteUnconfirmedAccountService>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            // for distributed cache
            services.AddDistributedMemoryCache();

            return services;
        }
    }
}


