namespace Sfko.Lego.Dto;

/// <summary>
/// An inventory details the contents of a particular set.
/// </summary>
public class Inventory
{
  /// <summary>
  /// The unique identifier of the set.
  /// </summary>
  /// <example>497-1</example>
  public required string SetNum { get; set; }

  /// <summary>
  /// The name of the set.
  /// </summary>
  /// <example>Galaxy Explorer</example>
  public required string SetName { get; set; }

  /// <summary>
  /// The year this set was issued.
  /// </summary>
  /// <example>1979</example>
  public long Year { get; set; }

  /// <summary>
  /// The number of parts in this set.
  /// </summary>
  /// <example>342</example>
  public long NumParts { get; set; }

  /// <summary>
  /// The name of the theme into which the set falls.
  /// </summary>
  /// <example>Classic Space</example>
  public required string ThemeName { get; set; }

  /// <summary>
  /// The parts that are included in this set.
  /// </summary>
  public IEnumerable<InventoryPart> Parts { get; set; } = Enumerable.Empty<InventoryPart>();
}

/// <summary>
/// Details of a specific part included in a set, across all colors in which it appears.
/// </summary>
public class InventoryPart
{
  /// <summary>
  /// The unique identifier of this part.
  /// </summary>
  /// <example>973p90c02</example>
  public required string PartNum { get; set; }

  /// <summary>
  /// The name of this part.
  /// </summary>
  /// <example>Torso Space Classic Moon Print / Red Arms / Red Hands</example>
  public required string PartName { get; set; }

  /// <summary>
  /// The name of this part's category.
  /// </summary>
  /// <example>Minifigs</example>
  public required string CategoryName { get; set; }

  /// <summary>
  /// The specific colors and quantities in which this part appears.
  /// </summary>
  public IEnumerable<InventoryPartVariant> Variants { get; set; } = Enumerable.Empty<InventoryPartVariant>();
}

/// <summary>
/// A specific color variation of a part found in a particular set.
/// </summary>
public class InventoryPartVariant
{
  /// <summary>
  /// The color of the part.
  /// </summary>
  /// <example>Red</example>
  public required string ColorName { get; set; }

  /// <summary>
  /// Is this color transparent?
  /// </summary>
  /// <example>false</example>
  public bool IsTrans { get; set; }

  /// <summary>
  /// The number of this part that are included.
  /// </summary>
  /// <example>2</example>
  public long Quantity { get; set; }

  /// <summary>
  /// Is this an excess part?
  /// </summary>
  /// <example>false</example>
  public bool IsSpare { get; set; }
}
