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
  /// <param name="setNum">The identifier of the set</param>
  /// <returns>Information about the parts in that set</returns>
  /// <response code="200">Returns information about the parts in that set</response>
  [HttpGet("set/{setNum:setNum}")]
  [Produces("application/json")]
  public async Task<Inventory> Set( string setNum )
  {
    _logger.LogDebug($"Fetching inventory for set {setNum}");
    var inventory = await GetInventory(setNum);
    if( null == inventory ) {
      _logger.LogDebug($"No inventory found for set {setNum}");
      throw new HttpRequestException("Set inventory not found", null, HttpStatusCode.NotFound);
    }

    _logger.LogDebug($"Fetching parts for inventory of set {setNum}");
    var parts = await _context.InventoryParts
      .Include(x => x.Part).Include(x => x.Part.Category).Include(x => x.Color)
      .Where(x => x.InventoryId == inventory.Id)
      .ToArrayAsync();

    _logger.LogDebug($"Building inventory for set {setNum} with {parts.Length} distinct parts");
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

  /// <summary>
  /// Fetches the parts inventory found in both of two specific sets.
  /// (That is, the intersection of the two sets of parts.)
  /// </summary>
  /// <param name="setNum1">The identifier of the first set</param>
  /// <param name="setNum2">The identifier of the second set</param>
  /// <returns>Information about the parts in both sets</returns>
  /// <response code="200">Returns information about the parts in both sets</response>
  [HttpGet("set/{setNum1:setNum}/and/{setNum2:setNum}")]
  [Produces("application/json")]
  public async Task<CommonInventory> SetIntersection( string setNum1, string setNum2 )
  {
    _logger.LogDebug($"Fetching inventory intersection for sets {setNum1} and {setNum2}");
    var inventory1 = await GetInventory(setNum1);
    var inventory2 = await GetInventory(setNum2);
    if( null == inventory1 || null == inventory2 ) {
      _logger.LogDebug($"No inventory found for set {setNum1} or {setNum2}");
      throw new HttpRequestException("Set inventory not found", null, HttpStatusCode.NotFound);
    }

    _logger.LogDebug($"Fetching parts for intersection of sets {setNum1} and {setNum2}");
    var parts = await _context.InventoryParts
      .Where(x => x.InventoryId == inventory1.Id && !x.IsSpare)
      .Join(
        _context.InventoryParts.Where(x => x.InventoryId == inventory2.Id && !x.IsSpare),
        p1 => new { p1.PartNum, p1.ColorId },
        p2 => new { p2.PartNum, p2.ColorId },
        (p1, p2) => new {
          PartNum = p1.PartNum,
          Part = p1.Part,
          Color = p1.Color,
          Quantity = Math.Min(p1.Quantity, p2.Quantity),
        }
      )
      .GroupBy(
        x => new {
          x.PartNum,
          PartName = x.Part.Name,
          CategoryName = x.Part.Category.Name,
        }, x => new {
          ColorName = x.Color.Name,
          IsTrans = x.Color.IsTrans,
          Quantity = x.Quantity,
          IsSpare = false,
        }
      )
      .Select(g => new InventoryPart {
        PartNum = g.Key.PartNum,
        PartName = g.Key.PartName,
        CategoryName = g.Key.CategoryName,
        Variants = g.Select(x => new InventoryPartVariant {
          ColorName = x.ColorName,
          IsTrans = x.IsTrans,
          Quantity = x.Quantity,
          IsSpare = x.IsSpare,
        }),
      })
      .ToArrayAsync();
    _logger.LogDebug($"Found {parts.Sum(p => p.Variants.Count())} variants of {parts.Length} distinct parts in both sets");

    var dto = new CommonInventory {
      Set1 = new BasicSetInfo {
        SetNum = inventory1.SetNum,
        SetName = inventory1.Set.Name,
        Year = inventory1.Set.Year,
        NumParts = inventory1.Set.NumParts,
        ThemeName = inventory1.Set.Theme.Name,
      },
      Set2 = new BasicSetInfo {
        SetNum = inventory2.SetNum,
        SetName = inventory2.Set.Name,
        Year = inventory2.Set.Year,
        NumParts = inventory2.Set.NumParts,
        ThemeName = inventory2.Set.Theme.Name,
      },
      Parts = parts,
    };

    return dto;
  }

  private Task<DbModel.Inventory?> GetInventory( string setNum )
  {
    return _context.Inventories
      .Include(x => x.Set).Include(x => x.Set.Theme)
      .Where(x => x.SetNum == setNum)
      .OrderByDescending(x => x.Version)
      .FirstOrDefaultAsync();
  }
}
