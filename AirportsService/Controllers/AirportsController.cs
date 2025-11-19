using AirportsService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AirportsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AirportsController : ControllerBase
{
    private readonly AirportsRepository _repository;

    public AirportsController(AirportsRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult GetAirports([FromQuery] int offset = 0, [FromQuery] int limit = 50)
    {
        offset = Math.Max(offset, 0);
        limit = limit <= 0 ? 50 : limit;
        limit = Math.Min(limit, 100);

        var items = _repository.GetPaged(offset, limit);

        var result = new
        {
            items,
            offset,
            limit,
            totalCount = _repository.Count
        };

        return Ok(result);
    }
}