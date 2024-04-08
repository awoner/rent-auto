namespace RentAutoPoc.Api.Infrastructure.Crons;

public interface ICronJob
{
    Task RunAsync(CancellationToken cancellationToken = default);
}