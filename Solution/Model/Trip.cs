namespace Solution.Model;

public class Trip
{
    public int IdTrip { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public ICollection<ClientTrip> ClientTrips { get; set; } = new List<ClientTrip>();
    public ICollection<Country> IdCountries { get; set; } = new List<Country>();
}