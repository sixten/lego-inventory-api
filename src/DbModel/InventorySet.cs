namespace Sfko.Lego.DbModel;

/// <summary>
/// A sub-set contained within another set.
/// </summary>
public partial class InventorySet
{
  /// <summary>
  /// The unique identifier of the <see cref="Inventory"/>.
  /// </summary>
  public long InventoryId { get; set; }

  /// <summary>
  /// The unique identifier of the <see cref="Set"/>.
  /// </summary>
  public required string SetNum { get; set; }

  /// <summary>
  /// The number of this sub-set included.
  /// </summary>
  public long Quantity { get; set; }

  /// <summary>
  /// The parent inventory.
  /// </summary>
  public required virtual Inventory Inventory { get; set; }

  /// <summary>
  /// The included sub-set.
  /// </summary>
  public required virtual Set Set { get; set; }
}
