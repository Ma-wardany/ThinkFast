using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineExam.Domain.Entities.Identity;

namespace OnlineExam.Service.BackgroundServices
{
    public class DeleteUnconfirmedAccountService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteUnconfirmedAccountService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Create a new scope to resolve scoped services
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                        var query = userManager.Users
                            .Where(user => !user.EmailConfirmed);

                        var result = await query.ExecuteDeleteAsync(stoppingToken);

                        Console.WriteLine($"{result} unconfirmed accounts deleted.");
                    }

                    // Wait before the next execution
                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
