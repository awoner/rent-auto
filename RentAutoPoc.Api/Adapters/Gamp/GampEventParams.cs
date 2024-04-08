namespace RentAutoPoc.Api.Services;

public class GampEventParams
{
    public string SessionId { get; set; }
    
    public int EngagementTimeMSec { get; set; }
    
    public int DebugMode { get; set; }
}

public class CurrencyParams : GampEventParams
{
    public float CurrencyRate { get; set; }
}