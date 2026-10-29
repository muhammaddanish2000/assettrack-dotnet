using AssetTrack.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AssetTrack.Api.Controllers;

[ApiController]
[Route("api/analytics")]
public class AnalyticsController :
    ControllerBase
{
    private readonly AssetService _service;

    public AnalyticsController(
        AssetService service
    )
    {
        _service = service;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> Summary()
    {
        object summary =
            await _service.GetSummaryAsync();

        return Ok(summary);
    }

    [HttpGet("categories")]
    public async Task<IActionResult>
        CategoryBreakdown()
    {
        object result =
            await _service
                .GetCategoryBreakdownAsync();

        return Ok(result);
    }
}
