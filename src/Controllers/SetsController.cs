using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sfko.Lego.Dto;

namespace Sfko.Lego.Controllers;

/// <summary>
/// Endpoints to provide information about sets.
/// </summary>
[ApiController]
[Route("sets")]
public class SetsController : ControllerBase
{
  private const int PAGE_SIZE = 100;

  private readonly DbModel.LegoContext _context;
  private readonly ILogger<SetsController> _logger;

  /// <summary>
  /// Initializes a new instance of this controller.
  /// </summary>
  /// <param name="context">A data context for set information</param>
  /// <param name="logger">A logger for this controller</param>
  public SetsController(DbModel.LegoContext context, ILogger<SetsController> logger )
  {
    _context = context;
    _logger = logger;
  }

  /// <summary>
  /// Fetches a paged list of the known sets.
  /// </summary>
  /// <param name="criteria">The search criteria to apply</param>
  /// <param name="page">A zero-indexed page number within the results</param>
  /// <returns>Information about the matching sets</returns>
  /// <response code="200">Returns information about the matching sets</response>
  [HttpGet("")]
  [Produces("application/json")]
  public async Task<SetListResponse> List( [FromQuery]SetSearchCriteria criteria, uint page = 0 )
  {
    var query = _context.Sets.AsQueryable();
    if( null != criteria ) {
      if( !string.IsNullOrEmpty(criteria.Name) ) {
        query = query.Where(x => x.Name.ToLower().Contains(criteria.Name.ToLower()));
      }
      if( criteria.Year.HasValue ) {
        query = query.Where(x => x.Year == criteria.Year.Value);
      }
      if( criteria.ThemeId.HasValue ) {
        query = query.Where(x => x.ThemeId == criteria.ThemeId.Value);
      }
      if( criteria.MinParts.HasValue && criteria.MinParts > 0 ) {
        query = query.Where(x => x.NumParts > criteria.MinParts.Value);
      }
    }

    _logger.LogDebug($"Fetching page {page} of sets");
    var sets = await query
      .OrderBy(x => x.Name)
      .Skip((int)page * PAGE_SIZE)
      .Take(PAGE_SIZE + 1)
      .Select(x => new Set {
        SetNum = x.SetNum,
        Name = x.Name,
        Year = x.Year,
        ThemeId = x.ThemeId,
        ThemeName = x.Theme.Name,
        NumParts = x.NumParts,
      })
      .ToArrayAsync();

    _logger.LogDebug($"Returning page {page} with {sets.Length} sets");
    return new SetListResponse {
      Sets = sets.Take(PAGE_SIZE).Select(x => {
        x.InventoryUrl = Url.Action("Set", "Inventories", new { setNum = x.SetNum });
        return x;
      }),
      PrevPageUrl = page > 0 ? Url.Action("List", ListRouteValues(criteria, page - 1)) : null,
      NextPageUrl = sets.Count() > PAGE_SIZE ? Url.Action("List", ListRouteValues(criteria, page + 1)) : null,
    };
  }

  private object ListRouteValues( SetSearchCriteria? criteria, uint page )
  {
    if( null == criteria ) {
      return new { page };
    }

    var criteriaParams = new RouteValueDictionary(criteria);
    criteriaParams[nameof(page)] = page;
    return criteriaParams;
  }

  /// <summary>
  /// Fetches a paged list of the sets containing the specified part.
  /// May optionally specify a specific color and minimum quantity for the part.
  /// </summary>
  /// <param name="partNum">The identifier of the part</param>
  /// <param name="colorId">An optional color in which the part must appear</param>
  /// <param name="minQuantity">An optional minimum quantity for the part</param>
  /// <param name="page">A zero-indexed page number within the results</param>
  /// <returns>Information about the matching sets</returns>
  /// <response code="200">Returns information about the matching sets</response>
  [HttpGet("containing/{partNum}")]
  [Produces("application/json")]
  public async Task<SetListResponse> Containing( string partNum, int? colorId = null, uint? minQuantity = null, uint page = 0 )
  {
    _logger.LogDebug($"Checking the existence of part {partNum}");
    var hasPart = await _context.Parts.AnyAsync(x => x.PartNum == partNum);
    if( !hasPart ) {
      _logger.LogDebug($"No part {partNum} found");
      throw new HttpRequestException("Part not found", null, HttpStatusCode.NotFound);
    }

    var query = _context.InventoryParts
      .Where(x => x.PartNum == partNum && !x.IsSpare);
    if( colorId.HasValue ) {
      query = query.Where(x => x.ColorId == colorId.Value);
    }
    if( minQuantity.HasValue && minQuantity.Value > 0 ) {
      query = query.Where(x => x.Quantity > minQuantity.Value);
    }

    _logger.LogDebug($"Fetching page {page} of sets containing part {partNum}");
    var sets = await query
      .Select(x => x.Inventory.Set)
      .Select(x => new Set {
        SetNum = x.SetNum,
        Name = x.Name,
        Year = x.Year,
        ThemeId = x.ThemeId,
        ThemeName = x.Theme.Name,
        NumParts = x.NumParts,
      })
      .Distinct()
      .OrderBy(x => x.Name)
      .Skip((int)page * PAGE_SIZE)
      .Take(PAGE_SIZE + 1)
      .ToArrayAsync();

    _logger.LogDebug($"Returning page {page} with {sets.Length} sets");
    return new SetListResponse {
      Sets = sets.Take(PAGE_SIZE).Select(x => {
        x.InventoryUrl = Url.Action("Set", "Inventories", new { setNum = x.SetNum });
        return x;
      }),
      PrevPageUrl = page > 0 ? Url.Action("Containing", new { partNum, colorId, minQuantity, page = page - 1 }) : null,
      NextPageUrl = sets.Count() > PAGE_SIZE ? Url.Action("Containing", new { partNum, colorId, minQuantity, page = page + 1 }) : null,
    };
  }
}
