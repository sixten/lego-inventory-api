using System.Globalization;
using System.Text.RegularExpressions;

namespace Sfko.Lego.Routing;

/// <summary>
/// A route constraint for set numbers.
/// </summary>
public class SetNumberRouteConstraint : IRouteConstraint
{
  private static readonly Regex _regex = new(
      @"^[-.a-z0-9]+$",
      RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled,
      TimeSpan.FromMilliseconds(100));

  /// <inheritdoc/>
  public bool Match( HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection )
  {
    if( !values.TryGetValue(routeKey, out var routeValue) ) {
      return false;
    }

    var routeValueString = Convert.ToString(routeValue, CultureInfo.InvariantCulture);

    if( routeValueString is null ) {
      return false;
    }

    return _regex.IsMatch(routeValueString);
  }
}
