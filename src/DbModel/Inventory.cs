namespace Sfko.Lego.DbModel;

/// <summary>
/// An inventory details the contents of a particular set.
/// </summary>
public partial class Inventory
{
  /// <summary>
  /// The unique identifier of this inventory.
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// Distinguishes multiple versions of this inventory, where available.
  /// </summary>
  public long Version { get; set; }

  /// <summary>
  /// The unique identifier of the <see cref="Set"/>.
  /// </summary>
  public required string SetNum { get; set; }

  /// <summary>
  /// The set that this inventory details.
  /// </summary>
  public required virtual Set Set { get; set; }

  /// <summary>
  /// The parts cataloged by this inventory.
  /// </summary>
  public virtual ICollection<InventoryPart> Parts { get; set; } = new List<InventoryPart>();

  /// <summary>
  /// The sub-sets cataloged by this inventory.
  /// </summary>
  public virtual ICollection<InventorySet> Sets { get; set; } = new List<InventorySet>();
}
