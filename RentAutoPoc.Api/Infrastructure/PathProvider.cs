namespace RentAutoPoc.Api.Infrastructure;

public class PathProvider
{
    public static string? GetBase()
    {
        //return Directory.GetParent(Directory.GetCurrentDirectory())?.FullName;
        return Directory.GetCurrentDirectory();
    }
}