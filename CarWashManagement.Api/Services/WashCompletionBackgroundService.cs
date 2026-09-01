using CarWashManagement.Api.Data;
using CarWashManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWashManagement.Api.Services;

public class WashCompletionBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public WashCompletionBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var context =
                scope.ServiceProvider.GetRequiredService<CarWashDbContext>();

            var activeTransactions = await context.WashTransactions
                .Include(transaction => transaction.WashStation)
                .Where(transaction =>
                    transaction.Status == WashTransactionStatus.InProgress)
                .ToListAsync(stoppingToken);

            foreach (var transaction in activeTransactions)
            {
                var duration =
                    WashProgramDuration.GetDuration(transaction.WashProgram);

                var finishTime =
                    transaction.StartedAt.Add(duration);

                if (DateTime.UtcNow >= finishTime)
                {
                    transaction.Status =
                        WashTransactionStatus.Completed;

                    transaction.CompletedAt =
                        DateTime.UtcNow;

                    transaction.WashStation.Status =
                        StationStatus.Available;
                }
            }

            await context.SaveChangesAsync(stoppingToken);

            await Task.Delay(
                TimeSpan.FromSeconds(10),
                stoppingToken
            );
        }
    }
}