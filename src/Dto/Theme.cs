namespace Sfko.Lego.Dto;

/// <summary>
/// A conceptual grouping of sets, like Town or Space.
/// </summary>
public class Theme
{
  /// <summary>
  /// The unique identifier for this theme.
  /// </summary>
  /// <example>50</example>
  public long Id { get; set; }

  /// <summary>
  /// The name of this theme.
  /// </summary>
  /// <example>Town</example>
  public required string Name { get; set; }

  /// <summary>
  /// Any subthemes of this theme.
  /// </summary>
  public ICollection<Theme> Subthemes { get; set; } = new List<Theme>();
}
