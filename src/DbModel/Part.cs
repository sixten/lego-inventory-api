namespace Sfko.Lego.DbModel;

/// <summary>
/// Details of a particular molded part.
/// </summary>
public partial class Part
{
  /// <summary>
  /// The unique identifier of this part.
  /// </summary>
  public required string PartNum { get; set; }

  /// <summary>
  /// The name of this part.
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  /// The unique identifier of the <see cref="Category"/>.
  /// </summary>
  public long PartCatId { get; set; }

  /// <summary>
  /// The category of this part.
  /// </summary>
  public required virtual PartCategory Category { get; set; }
}
