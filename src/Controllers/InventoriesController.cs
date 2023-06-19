using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sfko.Lego.Dto;

namespace Sfko.Lego.Controllers;

/// <summary>
/// Endpoints to provide information about parts inventories.
/// </summary>
[ApiController]
[Route("inventories")]
public class InventoriesController : ControllerBase
{
  private readonly DbModel.LegoContext _context;
  private readonly ILogger<InventoriesController> _logger;

  /// <summary>
  /// Initializes a new instance of this controller.
  /// </summary>
  /// <param name="context">A data context for inventory information</param>
  /// <param name="logger">A logger for this controller</param>
  public InventoriesController(DbModel.LegoContext context, ILogger<InventoriesController> logger )
  {
    _context = context;
    _logger = logger;
  }

  /// <summary>
  /// Fetches the parts inventory of a specific set.
  /// </summary>
  /// <param name="set_num">The identifier of the set</param>
  /// <returns>Information about the parts in that set</returns>
  /// <response code="200">Returns information about the parts in that set</response>
  [HttpGet("set/{set_num}")]
  [Produces("application/json")]
  public async Task<Inventory> Set( string set_num )
  {
    _logger.LogDebug($"Fetching inventory for set {set_num}");
    var inventory = await _context.Inventories
      .Include(x => x.Set).Include(x => x.Set.Theme)
      .Where(x => x.SetNum == set_num)
      .OrderByDescending(x => x.Version)
      .FirstOrDefaultAsync();
    if( null == inventory ) {
      _logger.LogDebug($"No inventory found for set {set_num}");
      throw new HttpRequestException("Set inventory not found", null, HttpStatusCode.NotFound);
    }

    _logger.LogDebug($"Fetching parts for inventory of set {set_num}");
    var parts = await _context.InventoryParts
      .Include(x => x.Part).Include(x => x.Part.Category).Include(x => x.Color)
      .Where(x => x.InventoryId == inventory.Id)
      .ToArrayAsync();

    _logger.LogDebug($"Building inventory for set {set_num} with {parts.Length} distinct parts");
    var dto = new Inventory {
      SetNum = inventory.SetNum,
      SetName = inventory.Set.Name,
      Year = inventory.Set.Year,
      NumParts = inventory.Set.NumParts,
      ThemeName = inventory.Set.Theme.Name,
      Parts = parts
        .GroupBy(x => x.Part, x => new InventoryPartVariant {
          ColorName = x.Color.Name,
          IsTrans = x.Color.IsTrans,
          Quantity = x.Quantity,
          IsSpare = x.IsSpare,
        })
        .Select(g => new InventoryPart {
          PartNum = g.Key.PartNum,
          PartName = g.Key.Name,
          CategoryName = g.Key.Category.Name,
          Variants = g.AsEnumerable(),
        })
        .AsEnumerable()
    };

    return dto;
  }
}
