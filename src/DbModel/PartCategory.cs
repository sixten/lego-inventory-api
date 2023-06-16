namespace Sfko.Lego.DbModel;

/// <summary>
/// A category that groups a set of parts.
/// </summary>
public partial class PartCategory
{
  /// <summary>
  /// The unique identifier of this category.
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// The name of this category.
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  /// The parts within this category.
  /// </summary>
  public virtual ICollection<Part> Parts { get; set; } = new List<Part>();
}
