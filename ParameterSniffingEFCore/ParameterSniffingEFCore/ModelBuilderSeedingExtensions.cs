using System;
using Microsoft.EntityFrameworkCore;

namespace ParameterSniffingEFCore
{
  /// <summary> This class is used to generates the initial  database records.</summary>
  public static class ModelBuilderSeedingExtensions
  {
    /// <summary>Seeds the specified model builder.</summary>
    /// <param name="modelBuilder">The model builder.</param>
    public static void Seed(this ModelBuilder modelBuilder)
    {
      var random = new Random();
      for (var i = 1; i < 150; i++)
      {
        modelBuilder.Entity<ProductInventory>().HasData(
          new ProductInventory
          {
            ProductInventoryId = i,
            Name = @"PlayStation 4",

            AxisCalibration = random.NextDouble() * 1000,
            Description =
              @"Test  Test DataTest DataTest DataTest ' DataTest DataTest DataTest DataTest DataTest DataTest @DataTest DataTest DataTest DataTest DataTest DataTest DataTest" + i + " DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest Data.",

            CreateDate = (IsOdd(i)) ? new DateTime(2017, 5, 5) : new DateTime(2018, 5, 5)
          }
        );
      }

      for (var i = 150; i < 100000; i++)
      {

        modelBuilder.Entity<ProductInventory>().HasData(
                          new ProductInventory
                          {

                            ProductInventoryId = i,
                            Name = "XBox One",
                            AxisCalibration = random.NextDouble() * 1000,
                            Description = @"Test  Test DataTest DataTest DataTest ' DataTest DataTest DataTest DataTest !DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest" + i + " DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest Data.",
                            CreateDate = (IsOdd(i)) ? DateTimeOffset.Now : new DateTime(2018, 5, 5)
                          });
      }

      for (var i = 100000; i < 101000; i++)
      {
        var name = Guid.NewGuid().ToString();
        modelBuilder.Entity<ProductInventory>().HasData(
          new ProductInventory
          {

            ProductInventoryId = i,
            Name = name,
            AxisCalibration = random.NextDouble() * 1000,
            Description = @"Test  Test DataTest DataTest DataTest ' DataTest DataTest DataTest DataTest !DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest" + name + " DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest DataTest Data.",
            CreateDate = (IsOdd(i)) ? DateTimeOffset.Now : new DateTime(2018, 5, 5)
          });
      }
    }

    /// <summary>Determines whether the specified value is odd.</summary>
    /// <param name="value">The value.</param>
    /// <returns>
    ///   <c>true</c> if the specified value is odd; otherwise, <c>false</c>.</returns>
    public static bool IsOdd(int value)
    {
      return value % 2 != 0;
    }
  }
}