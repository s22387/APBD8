namespace Solution.DTO;

public class AddClientToTripDto
{
    public int IdTrip { get; set; }
    public string TripName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public string Pesel { get; set; }
    public DateTime? PaymentDate { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}