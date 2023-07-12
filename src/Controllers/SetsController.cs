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
  /// <param name="page">A zero-indexed page number within the results</param>
  /// <returns>Information about the matching sets</returns>
  /// <response code="200">Returns information about the matching sets</response>
  [HttpGet("", Name = "List")]
  [Produces("application/json")]
  public async Task<SetListResponse> List( uint page = 0 )
  {
    var sets = await _context.Sets
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
    return new SetListResponse {
      Sets = sets.Take(PAGE_SIZE).Select(x => {
        x.InventoryUrl = Url.Action("Set", "Inventories", new { setNum = x.SetNum });
        return x;
      }),
      PrevPageUrl = page > 0 ? Url.Action("List", new { page = page - 1 }) : null,
      NextPageUrl = sets.Count() > PAGE_SIZE ? Url.Action("List", new { page = page + 1 }) : null,
    };
  }
}
