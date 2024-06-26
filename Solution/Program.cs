using Solution;
using Solution.DTO;
using Solution.Model;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/api/trips", async (Context dbContext) =>
{
    IQueryable<ICollection<Trip>> countries = dbContext.Countries.Select(x => x.IdTrips);
    var result = await dbContext.Trips
        .Include(x=> x.IdCountries)
        .Include(x => x.ClientTrips)
        .ThenInclude(x => x.IdClientNavigation)
        .Select(x => new TripDto
        {
            Name = x.Name,
            DateFrom = x.DateFrom,
            DateTo = x.DateTo,
            Description = x.Description,
            MaxPeople = x.MaxPeople,
            Countries = x.IdCountries.Select(x => new CountryDto { Name = x.Name }).ToList(),
            Clients = x.ClientTrips.Select(x => x.IdClientNavigation).Select(x => new ClientDto
            {
                FirstName = x.FirstName,
                LastName = x.LastName
            }).ToList()
        })
        .ToListAsync();

    return result;
});
app.MapDelete("/api/clients/{idClient:int}", async (Context dbContext, int idClient) =>
{
    Client? client = await dbContext.Clients
        .Include(x => x.ClientTrips)
        .FirstOrDefaultAsync(x => x.IdClient == idClient);
    if (client is null || !client.ClientTrips.Any())
    {
        return Results.BadRequest("Associated trips exist.");
    }

    await dbContext.SaveChangesAsync();
    return Results.Ok();
});
app.MapPost(
    "/api/trips/{idTrip:int}/clients",
    async (Context dbContext, int idTrip, AddClientToTripDto dto) =>
    {
        Trip? trip = await dbContext.Trips.FirstOrDefaultAsync(x => x.IdTrip == idTrip);
        if (trip is null)
        {
            return Results.BadRequest("Trip doesn't exist.");
        }
        
        Client? client = await dbContext.Clients
            .Include(x => x.ClientTrips)
            .FirstOrDefaultAsync(x => x.Pesel == dto.Pesel);
        if (client is null || !client.ClientTrips.Any())
        {
            return Results.BadRequest("No such client exists.");
        }

        bool doesTripExist = client.ClientTrips.Any(x => x.IdTrip == idTrip);
        if (!doesTripExist)
        {
            return Results.BadRequest("Client already assigned.");
        }

        await dbContext.SaveChangesAsync();
        
        return Results.Ok();
    }
);
app.Run();