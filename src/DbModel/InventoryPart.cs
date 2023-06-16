namespace Sfko.Lego.DbModel;

/// <summary>
/// A specific part included in an inventory.
/// </summary>
public partial class InventoryPart
{
  /// <summary>
  /// The unique identifier of the <see cref="Inventory"/>.
  /// </summary>
  public long InventoryId { get; set; }

  /// <summary>
  /// The unique identifier of the <see cref="Part"/>.
  /// </summary>
  public required string PartNum { get; set; }

  /// <summary>
  /// The unique identifier of the <see cref="Color"/>.
  /// </summary>
  public long ColorId { get; set; }

  /// <summary>
  /// The number of this part that are included.
  /// </summary>
  public long Quantity { get; set; }

  /// <summary>
  /// Is this an excess part?
  /// </summary>
  public bool IsSpare { get; set; }

  /// <summary>
  /// The color of this part.
  /// </summary>
  public required virtual Color Color { get; set; }

  /// <summary>
  /// The parent inventory including this part.
  /// </summary>
  public required virtual Inventory Inventory { get; set; }

  /// <summary>
  /// The details of the part.
  /// </summary>
  public required virtual Part Part { get; set; }
}
