using FlightsService.Grpc;
using Grpc.Core;

namespace FlightsService.Services
{
    public class FlightsServiceImpl : Flights.FlightsBase
    {
        private readonly List<FlightsService.Models.Flight> _flights;
        private static readonly Random Rand = new();

        public FlightsServiceImpl()
        {

            _flights = GenerateMockFlights(1000);
        }

        public override Task<GetFlightsResponse> GetFlights(GetFlightsRequest request, ServerCallContext context)
        {
            int offset = Math.Max(request.Offset, 0);
            int limit = request.Limit <= 0 ? 50 : request.Limit;
            limit = Math.Min(limit, 100);

            var paged = _flights
                .Skip(offset)
                .Take(limit)
                .ToList();

            var response = new GetFlightsResponse
            {
                Offset = offset,
                Limit = limit,
                TotalCount = _flights.Count
            };

            response.Flights.AddRange(paged.Select(f => new Flight
            {
                Id = f.Id,
                AircraftNumber = f.AircraftNumber,
                DepartureAirportCode = f.DepartureAirportCode,
                ArrivalAirportCode = f.ArrivalAirportCode,
                DepartureCity = f.DepartureCity,
                ArrivalCity = f.ArrivalCity,
                DepartureTime = f.DepartureTime.ToString("o"),
                ArrivalTime = f.ArrivalTime.ToString("o")
            }));

            return Task.FromResult(response);
        }

        private static List<FlightsService.Models.Flight> GenerateMockFlights(int count)
        {
            string[] cities = { "Milano", "Roma", "Firenze", "Parigi", "Londra", "Madrid", "Berlino", "New York", "Singapore", "Tokyo" };
            string[] airports = { "MXP", "FCO", "FLR", "CDG", "LHR", "MAD", "BER", "JFK", "SIN", "HND" };
            string[] aircrafts = { "A320", "A330", "A350", "B737", "B747", "B777", "B787" };

            var flights = new List<FlightsService.Models.Flight>();

            for (int i = 0; i < count; i++)
            {
                var depIndex = Rand.Next(cities.Length);
                var arrIndex = Rand.Next(cities.Length);

                flights.Add(new FlightsService.Models.Flight
                {
                    Id = $"FL{i:D4}",
                    AircraftNumber = aircrafts[Rand.Next(aircrafts.Length)],
                    DepartureAirportCode = airports[depIndex],
                    ArrivalAirportCode = airports[arrIndex],
                    DepartureCity = cities[depIndex],
                    ArrivalCity = cities[arrIndex],
                    DepartureTime = DateTime.UtcNow.AddMinutes(Rand.Next(0, 20000)),
                    ArrivalTime = DateTime.UtcNow.AddMinutes(Rand.Next(20000, 40000))
                });
            }

            return flights;
        }
    }
}
