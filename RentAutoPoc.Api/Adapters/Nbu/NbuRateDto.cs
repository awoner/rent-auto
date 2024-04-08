namespace RentAutoPoc.Api.Services;

public class NbuRateDto
{
    public string StartDate { get; set; }

    public string TimeSign { get; set; }

    public string CurrencyCode { get; set; }

    public string CurrencyCodeL { get; set; }

    public int Units { get; set; }

    public decimal Amount { get; set; }
}