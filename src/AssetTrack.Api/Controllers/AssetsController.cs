using AssetTrack.Api.DTOs;
using AssetTrack.Api.Models;
using AssetTrack.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AssetTrack.Api.Controllers;

[ApiController]
[Route("api/assets")]
public class AssetsController : ControllerBase
{
    private readonly AssetService _service;

    public AssetsController(
        AssetService service
    )
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAssets(
        [FromQuery] string? search,
        [FromQuery] string? category,
        [FromQuery] AssetStatus? status
    )
    {
        List<Asset> assets =
            await _service.GetAssetsAsync(
                search,
                category,
                status
            );

        return Ok(assets);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id
    )
    {
        Asset? asset =
            await _service.GetByIdAsync(id);

        if (asset is null)
        {
            return NotFound(
                new
                {
                    message =
                        "Asset not found."
                }
            );
        }

        return Ok(asset);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody]
        AssetCreateRequest request
    )
    {
        try
        {
            Asset asset =
                await _service.CreateAsync(
                    request
                );

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = asset.Id
                },
                asset
            );
        }
        catch (
            InvalidOperationException exception
        )
        {
            return Conflict(
                new
                {
                    message =
                        exception.Message
                }
            );
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody]
        AssetUpdateRequest request
    )
    {
        try
        {
            Asset? asset =
                await _service.UpdateAsync(
                    id,
                    request
                );

            if (asset is null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Asset not found."
                    }
                );
            }

            return Ok(asset);
        }
        catch (
            InvalidOperationException exception
        )
        {
            return Conflict(
                new
                {
                    message =
                        exception.Message
                }
            );
        }
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromQuery]
        AssetStatus status
    )
    {
        Asset? asset =
            await _service.UpdateStatusAsync(
                id,
                status
            );

        if (asset is null)
        {
            return NotFound(
                new
                {
                    message =
                        "Asset not found."
                }
            );
        }

        return Ok(asset);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id
    )
    {
        bool deleted =
            await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(
                new
                {
                    message =
                        "Asset not found."
                }
            );
        }

        return NoContent();
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export()
    {
        byte[] file =
            await _service.ExportCsvAsync();

        string filename =
            $"assets-{DateTime.UtcNow:yyyy-MM-dd}.csv";

        return File(
            file,
            "text/csv",
            filename
        );
    }
}
