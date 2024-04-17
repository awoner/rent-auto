using RentAutoPoc.Api.Infrastructure;

namespace RentAutoPoc.Api.Application.Services;

public interface IImageService
{
    Task<byte[]> GetImage(string name, CancellationToken cancellationToken);
}

public class LocalImageService : IImageService
{
    private readonly string _imagesPath = @$"{PathProvider.GetBase()}/Resources/Images";

    public Task<byte[]> GetImage(string name, CancellationToken cancellationToken)
    {
        return File.ReadAllBytesAsync( @$"{_imagesPath}/{name}" , cancellationToken);
    }

    public Task AddImage(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}