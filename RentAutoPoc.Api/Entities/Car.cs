namespace RentAutoPoc.Api.Entities;

public class Car
{
    public Guid Id { get; set; }
    
    public string Model { get; set; }
    
    public int ModelYear { get; set; }
}