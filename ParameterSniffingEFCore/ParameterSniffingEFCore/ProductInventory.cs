using System;

namespace ParameterSniffingEFCore
{
  /// <summary>
  /// ProductInventory POCO.
  /// </summary>
  public class ProductInventory
  {
    /// <summary>Gets or sets the product inventory identifier.</summary>
    /// <value>The product inventory identifier.</value>
    public long ProductInventoryId { get; set; }

    /// <summary>Gets or sets the name.</summary>
    /// <value>The name.</value>
    public string Name { get; set; }

    /// <summary>Gets or sets the axis calibration.</summary>
    /// <value>The axis calibration.</value>
    public double AxisCalibration { get; set; }

    /// <summary>Gets or sets the description.</summary>
    /// <value>The description.</value>
    public string Description { get; set; }

    /// <summary>Gets or sets the create date.</summary>
    /// <value>The create date.</value>
    public DateTimeOffset CreateDate { get; set; }
  }
}