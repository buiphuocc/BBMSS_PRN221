using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class BookingStatusUpdater : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BookingStatusUpdater> _logger;
        private Timer _timer;

        public BookingStatusUpdater(IServiceProvider serviceProvider, ILogger<BookingStatusUpdater> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Booking Status Updater starting.");
            _timer = new Timer(UpdateBookingStatuses, null, TimeSpan.Zero, TimeSpan.FromMinutes(1)); // Check every minute
            return Task.CompletedTask;
        }

        private void UpdateBookingStatuses(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();
                bookingService.UpdateBookingsStatusBasedOnCurrentTime();
            }

            _logger.LogInformation("Booking statuses updated.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Booking Status Updater stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
