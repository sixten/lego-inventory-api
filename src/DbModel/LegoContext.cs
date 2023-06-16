using Microsoft.EntityFrameworkCore;

namespace Sfko.Lego.DbModel;

/// <summary>
/// The data context for LEGO parts inventory information.
/// </summary>
public partial class LegoContext : DbContext
{
  /// <summary>
  /// Initializes a new instance of this context.
  /// </summary>
  public LegoContext()
  {
  }

  /// <summary>
  /// Initializes a new instance of this context with the given options.
  /// </summary>
  /// <param name="options">The options for this context.</param>
  public LegoContext( DbContextOptions<LegoContext> options )
      : base(options)
  {
  }

  /// <summary>
  /// The set of colors in which parts can be manufactured.
  /// </summary>
  public virtual DbSet<Color> Colors { get; set; } = null!;

  /// <summary>
  /// The set of inventories of known sets.
  /// </summary>
  public virtual DbSet<Inventory> Inventories { get; set; } = null!;

  /// <summary>
  /// The set of parts contained in various inventories.
  /// </summary>
  public virtual DbSet<InventoryPart> InventoryParts { get; set; } = null!;

  /// <summary>
  /// The set of sets contained in various inventories.
  /// </summary>
  public virtual DbSet<InventorySet> InventorySets { get; set; } = null!;

  /// <summary>
  /// The set of known distinct LEGO parts.
  /// </summary>
  public virtual DbSet<Part> Parts { get; set; } = null!;

  /// <summary>
  /// The set of categories into which parts are divided.
  /// </summary>
  public virtual DbSet<PartCategory> PartCategories { get; set; } = null!;

  /// <summary>
  /// The set of known LEGO building sets.
  /// </summary>
  public virtual DbSet<Set> Sets { get; set; } = null!;

  /// <summary>
  /// The set of themes into which sets are categorized.
  /// </summary>
  public virtual DbSet<Theme> Themes { get; set; } = null!;

  /// <inheritdoc/>
  protected override void OnModelCreating( ModelBuilder modelBuilder )
  {
    modelBuilder.Entity<Color>(entity => {
      entity.ToTable("colors");

      entity.HasIndex(e => e.Name, "IX_colors_name").IsUnique();

      entity.Property(e => e.Id)
          .ValueGeneratedNever()
          .HasColumnName("id");
      entity.Property(e => e.IsTrans).HasColumnName("is_trans");
      entity.Property(e => e.Name).HasColumnName("name");
      entity.Property(e => e.Rgb).HasColumnName("rgb");
    });

    modelBuilder.Entity<Inventory>(entity => {
      entity.ToTable("inventories");

      entity.Property(e => e.Id)
          .ValueGeneratedNever()
          .HasColumnName("id");
      entity.Property(e => e.SetNum).HasColumnName("set_num");
      entity.Property(e => e.Version).HasColumnName("version");

      entity.HasOne(d => d.Set).WithMany(p => p.Inventories)
          .HasForeignKey(d => d.SetNum)
          .OnDelete(DeleteBehavior.ClientSetNull);
    });

    modelBuilder.Entity<InventoryPart>(entity => {
      entity.HasKey(e => new { e.InventoryId, e.PartNum });

      entity.ToTable("inventory_parts");

      entity.Property(e => e.ColorId).HasColumnName("color_id");
      entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
      entity.Property(e => e.IsSpare).HasColumnName("is_spare");
      entity.Property(e => e.PartNum).HasColumnName("part_num");
      entity.Property(e => e.Quantity).HasColumnName("quantity");

      entity.HasOne(d => d.Color).WithMany()
          .HasForeignKey(d => d.ColorId)
          .OnDelete(DeleteBehavior.ClientSetNull);

      entity.HasOne(d => d.Inventory).WithMany(p => p.Parts)
          .HasForeignKey(d => d.InventoryId)
          .OnDelete(DeleteBehavior.ClientSetNull);

      entity.HasOne(d => d.Part).WithMany()
          .HasForeignKey(d => d.PartNum)
          .OnDelete(DeleteBehavior.ClientSetNull);
    });

    modelBuilder.Entity<InventorySet>(entity => {
      entity.HasKey(e => new { e.InventoryId, e.SetNum });

      entity.ToTable("inventory_sets");

      entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
      entity.Property(e => e.Quantity).HasColumnName("quantity");
      entity.Property(e => e.SetNum).HasColumnName("set_num");

      entity.HasOne(d => d.Inventory).WithMany(p => p.Sets)
          .HasForeignKey(d => d.InventoryId)
          .OnDelete(DeleteBehavior.ClientSetNull);

      entity.HasOne(d => d.Set).WithMany()
          .HasForeignKey(d => d.SetNum)
          .OnDelete(DeleteBehavior.ClientSetNull);
    });

    modelBuilder.Entity<Part>(entity => {
      entity.HasKey(e => e.PartNum);

      entity.ToTable("parts");

      entity.Property(e => e.PartNum).HasColumnName("part_num");
      entity.Property(e => e.Name).HasColumnName("name");
      entity.Property(e => e.PartCatId).HasColumnName("part_cat_id");

      entity.HasOne(d => d.Category).WithMany(p => p.Parts)
          .HasForeignKey(d => d.PartCatId)
          .OnDelete(DeleteBehavior.ClientSetNull);
    });

    modelBuilder.Entity<PartCategory>(entity => {
      entity.ToTable("part_categories");

      entity.Property(e => e.Id)
          .ValueGeneratedNever()
          .HasColumnName("id");
      entity.Property(e => e.Name).HasColumnName("name");
    });

    modelBuilder.Entity<Set>(entity => {
      entity.HasKey(e => e.SetNum);

      entity.ToTable("sets");

      entity.Property(e => e.SetNum).HasColumnName("set_num");
      entity.Property(e => e.Name).HasColumnName("name");
      entity.Property(e => e.NumParts).HasColumnName("num_parts");
      entity.Property(e => e.ThemeId).HasColumnName("theme_id");
      entity.Property(e => e.Year).HasColumnName("year");

      entity.HasOne(d => d.Theme).WithMany(p => p.Sets)
          .HasForeignKey(d => d.ThemeId)
          .OnDelete(DeleteBehavior.ClientSetNull);
    });

    modelBuilder.Entity<Theme>(entity => {
      entity.ToTable("themes");

      entity.Property(e => e.Id)
          .ValueGeneratedNever()
          .HasColumnName("id");
      entity.Property(e => e.Name).HasColumnName("name");
      entity.Property(e => e.ParentThemeId).HasColumnName("parent_id");

      entity.HasOne(d => d.ParentTheme).WithMany(p => p.Subthemes).HasForeignKey(d => d.ParentThemeId);
    });

    OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial( ModelBuilder modelBuilder );
}
