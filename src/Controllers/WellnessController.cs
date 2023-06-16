using Microsoft.AspNetCore.Mvc;
using Sfko.Lego.Dto;

namespace Sfko.Lego.Controllers;

[ApiController]
[Route("wellness")]
public class WellnessController : ControllerBase
{
  private readonly ILogger<WellnessController> _logger;

  public WellnessController( ILogger<WellnessController> logger )
  {
    _logger = logger;
  }

  [HttpGet(Name = "Check")]
  public Wellness Check()
  {
    return new Wellness("👍");
  }
}
