namespace Sfko.Lego.Dto;

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
