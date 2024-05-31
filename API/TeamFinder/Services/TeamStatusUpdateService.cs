using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamFinder.Models;
using TeamFinder.Models.Domain;
using TeamFinder.Data;

namespace TeamFinder.Services
{
    public class TeamStatusUpdateService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TeamStatusUpdateService> _logger;
        private Timer? _timer;

        public TeamStatusUpdateService(IServiceProvider serviceProvider, ILogger<TeamStatusUpdateService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(TeamStatusUpdateService)} is starting.");
            _timer = new Timer(UpdateTeamStatuses, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
            return Task.CompletedTask;
        }

        private void UpdateTeamStatuses(object? state)
        {
            _logger.LogInformation($"{nameof(TeamStatusUpdateService)} is running the update process.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                try
                {
                    var teamsToUpdate = dbContext.Teams
                        .Where(t => t.AcceptedToActivity == RequestStatus.Pending && t.ActivityRegistered.StartDate <= DateTime.UtcNow)
                        .ToList();

                    foreach (var team in teamsToUpdate)
                    {
                        team.AcceptedToActivity = RequestStatus.Rejected;
                        _logger.LogInformation($"Team {team.Name} (ID: {team.Id}) status updated to Rejected.");
                    }

                    dbContext.SaveChanges();
                    _logger.LogInformation($"{nameof(TeamStatusUpdateService)} successfully updated team statuses.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating team statuses.");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(TeamStatusUpdateService)} is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _logger.LogInformation($"{nameof(TeamStatusUpdateService)} is disposing.");
        }
    }
}
