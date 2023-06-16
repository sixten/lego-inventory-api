namespace Sfko.Lego.Dto;

public class Wellness
{
  public string Status { get; }

  public uint ApiVersion { get; }

  public string? AssemblyVersion { get; }

  public Wellness( string status, uint version = 1 )
  {
    Status = status;
    ApiVersion = version;
    AssemblyVersion = GetType().Assembly.GetName().Version?.ToString();
  }
}
