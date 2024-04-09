namespace RentAutoPoc.Api.Services;

public class GampRequest<T> where T : GampEventParams
{
    public string ClientId { get; set; }

    public string UserId { get; set; }

    public bool NonPersonalizedAds { get; set; }

    public IEnumerable<GampEvent<T>> Events { get; set; }
}

public class GampEvent<T> where T : GampEventParams
{
    public string Name { get; set; }
    
    public T Params { get; set; }
    
}