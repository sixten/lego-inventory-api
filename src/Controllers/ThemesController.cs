using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Sfko.Lego.Dto;
using static Sfko.Lego.DbModel.QueryExtensions;

namespace Sfko.Lego.Controllers;

/// <summary>
/// Endpoints to provide information about themes.
/// </summary>
[ApiController]
[Route("themes")]
public class ThemesController : ControllerBase
{
  private readonly DbModel.LegoContext _context;
  private readonly ILogger<ThemesController> _logger;

  /// <summary>
  /// Initializes a new instance of this controller.
  /// </summary>
  /// <param name="context">A data context for theme information</param>
  /// <param name="logger">A logger for this controller</param>
  public ThemesController( DbModel.LegoContext context, ILogger<ThemesController> logger )
  {
    _context = context;
    _logger = logger;
  }

  /// <summary>
  /// Fetches the set of all themes.
  /// </summary>
  /// <returns>A forest of <see cref="Theme"/> objects</returns>
  /// <response code="200">Returns the forest of all themes</response>
  [HttpGet("")]
  [Produces("application/json")]
  public async Task<IEnumerable<Theme>> FullTree()
  {
    _logger.LogDebug($"Fetching entire theme hierarchy");
    return await FetchThemes(_context.Themes, rootId: null);
  }

  /// <summary>
  /// Fetches the set of all subthemes of a specified root theme.
  /// </summary>
  /// <param name="rootId">The identifier of a root theme</param>
  /// <returns>A forest of <see cref="Theme"/> objects</returns>
  /// <exception cref="HttpRequestException"></exception>
  /// <response code="200">Returns the tree of all themes</response>
  /// <response code="404">The specified root theme does not exist</response>
  [HttpGet("{rootId:long}")]
  [Produces("application/json")]
  public async Task<IEnumerable<Theme>> Subtree( long rootId )
  {
    if( !await _context.Themes.AnyAsync(x => x.Id == rootId) ) {
      throw new HttpRequestException("Root theme not found", null, HttpStatusCode.NotFound);
    }

    _logger.LogDebug($"Fetching subthemes of {rootId}");
    return await FetchThemes(_context.Themes.HierarchicalFromRoot(rootId), rootId);
  }

  /// <summary>
  /// Fetches a set of themes from persistent storage, and transforms them into
  /// a set of hierarchical <see cref="Theme"/> objects.
  /// </summary>
  /// <param name="query">A query to fetch a set of themes</param>
  /// <param name="rootId">The parent identifier of the top level of results</param>
  /// <returns>A forest of <see cref="Theme"/> objects</returns>
  private async Task<IEnumerable<Theme>> FetchThemes( IQueryable<DbModel.Theme> query, long? rootId )
  {
    var themes = await query
      .OrderBy(x => x.ParentThemeId).ThenBy(x => x.Name)
      .ToArrayAsync();
    _logger.LogDebug($"Query for themes found {themes.Length} results");

    var themeIndex = new Dictionary<long, Theme>();
    var results = new List<Theme>();

    foreach( var entity in themes ) {
      var theme = new Theme {
        Id = entity.Id,
        Name = entity.Name,
      };
      if( entity.ParentThemeId == rootId ) {
        results.Add(theme);
      }
      else if( entity.ParentThemeId.HasValue && themeIndex.ContainsKey(entity.ParentThemeId.Value) ) {
        themeIndex[entity.ParentThemeId.Value].Subthemes.Add(theme);
      }
      themeIndex[entity.Id] = theme;
    }

    return results;
  }
}
