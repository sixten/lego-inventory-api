namespace Sfko.Lego.DbModel;

/// <summary>
/// A conceptual grouping of sets, like City or Space.
/// </summary>
public partial class Theme
{
  /// <summary>
  /// The unique identifier for this theme.
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// The name of this theme.
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  /// The unique identifier of the <see cref="ParentTheme"/>.
  /// </summary>
  public long? ParentThemeId { get; set; }

  /// <summary>
  /// Any subthemes of this theme.
  /// </summary>
  public virtual ICollection<Theme> Subthemes { get; set; } = new List<Theme>();

  /// <summary>
  /// The parent theme of this theme.
  /// </summary>
  public virtual Theme? ParentTheme { get; set; }

  /// <summary>
  /// The individual sets within this theme.
  /// </summary>
  public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}
