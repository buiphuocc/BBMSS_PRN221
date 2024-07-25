namespace Services.Interfaces
{
    public interface IBookingStatusUpdater
    {
        void Dispose();
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}