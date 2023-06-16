namespace Sfko.Lego.DbModel;

/// <summary>
/// Details of a particular color in which parts can be produced.
/// </summary>
public partial class Color
{
  /// <summary>
  /// The unique identifier of this color.
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// The name of this color.
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  /// An approximate HTML hex color code for this color.
  /// </summary>
  public required string Rgb { get; set; }

  /// <summary>
  /// Is this color transparent?
  /// </summary>
  public bool IsTrans { get; set; }
}
