using Microsoft.AspNetCore.Mvc;
using Sfko.Lego.Dto;

namespace Sfko.Lego.Controllers;

/// <summary>
/// Endpoint to provide health information about the application.
/// </summary>
[ApiController]
[Route("wellness")]
public class WellnessController : ControllerBase
{
  private readonly ILogger<WellnessController> _logger;

  /// <summary>
  /// Initializes a new instance of this controller.
  /// </summary>
  /// <param name="logger">A logger for the controller</param>
  public WellnessController( ILogger<WellnessController> logger )
  {
    _logger = logger;
  }

  /// <summary>
  /// Provides current health information about the application.
  /// </summary>
  /// <returns>A <see cref="Wellness" /> containing health information.</returns>
  /// <response code="200">Returns health information</response>
  [HttpGet(Name = "Check")]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public Wellness Check()
  {
    return new Wellness("👍");
  }
}
