namespace Sfko.Lego.Dto;

/// <summary>
/// Summarizes the status of the API application.
/// </summary>
public class Wellness
{
  /// <summary>
  /// The current status of the API.
  /// </summary>
  public string Status { get; }

  /// <summary>
  /// The current version of the API.
  /// </summary>
  public uint ApiVersion { get; }

  /// <summary>
  /// The version of the assembly the application was loaded from.
  /// </summary>
  public string? AssemblyVersion { get; }

  /// <summary>
  /// Initializes a new instance of this class with the given status and API version.
  /// </summary>
  /// <param name="status">The status description</param>
  /// <param name="version">An optional API version (defaulting to 1).</param>
  public Wellness( string status, uint version = 1 )
  {
    Status = status;
    ApiVersion = version;
    AssemblyVersion = GetType().Assembly.GetName().Version?.ToString();
  }
}
