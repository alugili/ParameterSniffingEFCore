using Microsoft.EntityFrameworkCore;

namespace ParameterSniffingEFCore
{
  /// <summary>
  /// Db Context for the demo.
  /// </summary>
  public class ProductInventoryDbContext : DbContext
  {

    /// <summary>Gets or sets the product inventories.</summary>
    /// <value>The product inventories.</value>
    public DbSet<ProductInventory> ProductInventories { get; set; }

    /// <summary>
    /// Override this method to further configure the model that was discovered by convention from the entity types
    /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1"/> properties on your derived context. The resulting model may be cached
    /// and re-used for subsequent instances of your derived context.
    /// </summary>
    /// <param name="modelBuilder">
    /// The builder being used to construct the model for this context. Databases (and other extensions) typically
    /// define extension methods on this object that allow you to configure aspects of the model that are specific
    /// to a given database.
    /// </param>
    /// <remarks>
    /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)"/>)
    /// then this method will not be run.
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<ProductInventory>().HasIndex(x => x.Name);
      modelBuilder.Entity<ProductInventory>().HasIndex(x => x.CreateDate);

      modelBuilder.Seed();
    }

    /// <summary>
    ///   <para>
    /// Override this method to configure the database (and other options) to be used for this context.
    /// This method is called for each instance of the context that is created.
    /// The base implementation does nothing.
    /// </para>
    ///   <para>
    /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions"/> may or may not have been passed
    /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured"/> to determine if
    /// the options have already been set, and skip some or all of the logic in
    /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)"/>.
    /// </para>
    /// </summary>
    /// <param name="optionsBuilder">
    /// A builder used to create or modify options for this context. Databases (and other extensions)
    /// typically define extension methods on this object that allow you to configure the context.
    /// </param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer("server=.;database=ProductInventoryDatabase;trusted_connection=true;");
    }
  }
}