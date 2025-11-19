using AirportsService.Models;

namespace AirportsService.Services;

public class AirportsRepository
{
    private readonly List<Airport> _airports;
    private static readonly Random Rand = new();

    public AirportsRepository()
    {
        _airports = GenerateMockAirports(300);
    }

    public IEnumerable<Airport> GetPaged(int offset, int limit)
        => _airports
            .Skip(offset)
            .Take(limit);

    public int Count => _airports.Count;

    private static List<Airport> GenerateMockAirports(int count)
    {
        string[] cities = { "Milano", "Roma", "Firenze", "Parigi", "Londra", "Madrid", "Berlino", "New York", "Singapore", "Tokyo", "Los Angeles", "Chicago" };
        string[] countries = { "Italy", "France", "UK", "Spain", "Germany", "USA", "Singapore", "Japan" };

        var list = new List<Airport>();

        for (int i = 0; i < count; i++)
        {
            var city = cities[Rand.Next(cities.Length)];
            var country = countries[Rand.Next(countries.Length)];

            list.Add(new Airport
            {
                Id = $"AP{i:D4}",
                Name = $"{city} International Airport {i}",
                City = city,
                Country = country
            });
        }

        return list;
    }
}