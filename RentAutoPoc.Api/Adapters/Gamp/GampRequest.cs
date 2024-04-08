namespace RentAutoPoc.Api.Services;

public class GampRequest
{
    public string ClientId { get; set; }

    public string UserId { get; set; }

    public bool NonPersonalizedAds { get; set; }

    public IEnumerable<GampEvent> Events { get; set; }
}

public class GampEvent
{
    public string Name { get; set; }
    
    public GampEventParams Params { get; set; }
    
}