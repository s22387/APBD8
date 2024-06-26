namespace Solution.Model;

public class Country
{
    public int IdCountry { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Trip> IdTrips { get; set; } = new List<Trip>();
}