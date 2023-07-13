namespace Sfko.Lego.Dto;

/// <summary>
/// The collection of search criteria that can be applied to the set list.
/// </summary>
public class SetSearchCriteria
{
  /// <summary>
  /// The set name, or part of it.
  /// </summary>
  /// <example>explore</example>
  public string? Name { get; set; }

  /// <summary>
  /// The year the set was issued.
  /// </summary>
  /// <example>1979</example>
  public int? Year { get; set; }

  /// <summary>
  /// The unique identifier of the theme to which the set belongs.
  /// </summary>
  /// <example>130</example>
  public int? ThemeId { get; set; }

  /// <summary>
  /// The minimum number of parts in the set.
  /// </summary>
  /// <example>342</example>
  public uint? MinParts { get; set; }
}

/// <summary>
/// The response from the set list endpoint.
/// </summary>
public class SetListResponse
{
  /// <summary>
  /// A page of sets matching the query criteria.
  /// </summary>
  public IEnumerable<Set> Sets { get; set; } = Enumerable.Empty<Set>();

  /// <summary>
  /// The URL for the next page of results.
  /// </summary>
  public string? NextPageUrl { get; set; }

  /// <summary>
  /// The URL for the previous page of results.
  /// </summary>
  public string? PrevPageUrl { get; set; }
}
