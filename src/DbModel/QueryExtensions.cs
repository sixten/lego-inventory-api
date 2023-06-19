using Microsoft.EntityFrameworkCore;

namespace Sfko.Lego.DbModel;

/// <summary>
/// Extension methods encapsulating more complex query logic.
/// </summary>
public static class QueryExtensions
{
  /// <summary>
  /// Defines a query that will fetch the theme hierarchy descending from a
  /// particular root theme.
  /// </summary>
  /// <param name="themes">The root theme collection</param>
  /// <param name="rootId">The unique identifier of the root theme</param>
  /// <returns>An <see cref="IQueryable{Theme}"/> resolving to the specified themes</returns>
  public static IQueryable<Theme> HierarchicalFromRoot( this DbSet<Theme> themes, long rootId )
  {
    return themes.FromSql($"""
      WITH RECURSIVE theme_hierarchy(id, name, parent_id) AS (
        select id, name, parent_id
        from themes
        where parent_id = {rootId}

        union
        select themes.id, themes.name, themes.parent_id
        from themes
          join theme_hierarchy on themes.parent_id = theme_hierarchy.id
      )
      select id, name, parent_id from theme_hierarchy
      """);
  }
}
