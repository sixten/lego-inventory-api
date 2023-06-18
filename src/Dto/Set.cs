﻿namespace Sfko.Lego.Dto;

/// <summary>
/// A specific packaged set of parts.
/// </summary>
public class Set
{
  /// <summary>
  /// The unique identifier of this set.
  /// </summary>
  /// <example>497-1</example>
  public required string SetNum { get; set; }

  /// <summary>
  /// The name of the set.
  /// </summary>
  /// <example>Galaxy Explorer</example>
  public required string Name { get; set; }

  /// <summary>
  /// The year this set was issued.
  /// </summary>
  /// <example>1979</example>
  public long Year { get; set; }

  /// <summary>
  /// The unique identifier of the theme to which this set belongs.
  /// </summary>
  /// <example>130</example>
  public long ThemeId { get; set; }

  /// <summary>
  /// The number of parts in this set.
  /// </summary>
  /// <example>342</example>
  public long NumParts { get; set; }
}
