namespace Sfko.Lego.DbModel;

/// <summary>
/// A specific packaged set of parts.
/// </summary>
public partial class Set
{
  /// <summary>
  /// The unique identifier of this set.
  /// </summary>
  public required string SetNum { get; set; }

  /// <summary>
  /// The name of the set.
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  /// The year this set was issued.
  /// </summary>
  public long Year { get; set; }

  /// <summary>
  /// The unique identifier of the <see cref="Theme"/>.
  /// </summary>
  public long ThemeId { get; set; }

  /// <summary>
  /// The number of parts in this set.
  /// </summary>
  public long NumParts { get; set; }

  /// <summary>
  /// The parts inventories logged for this set.
  /// </summary>
  public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

  /// <summary>
  /// The theme to which this set belongs.
  /// </summary>
  public required virtual Theme Theme { get; set; }
}
