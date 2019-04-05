using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ParameterSniffingEFCore
{
  public class Program
  {
    static void Main(string[] args)
    {
      // Creating the database.
      using (var productInventoryDbContext = new ProductInventoryDbContext())
      {
        productInventoryDbContext.Database.EnsureDeleted();
        productInventoryDbContext.Database.EnsureCreated();

        // Enable Query Store to Generate the Query Plans!
        productInventoryDbContext.Database.ExecuteSqlCommand(
          @"ALTER DATABASE ProductInventoryDatabase
                           SET QUERY_STORE = ON   
                           (  
                             OPERATION_MODE = READ_WRITE
                           )"
        );
      }

      Console.WriteLine("_________________________  Begin The Tests! _________________________");

      FindFirstSmallDataAmountThenBigDataAmount();


      Console.WriteLine("_________________________  Next Test! _________________________");

      FindFirstBigDataAmountThenSmallDataAmount();

      Console.WriteLine("_________________________  Done! _________________________");
    }

    /// <summary>Finds the first small data amount then the big data amount.</summary>
    private static void FindFirstSmallDataAmountThenBigDataAmount()
    {
      using (var productInventoryDbContext = new ProductInventoryDbContext())
      {
        Console.WriteLine("************* Find small amount of Data 2017!******************");

        // Command listener to add "Recompile" to the query.
        var listener = productInventoryDbContext.GetService<DiagnosticSource>();
        (listener as DiagnosticListener).SubscribeWithAdapter(new QueryCommandListener());

        var firstDate = new DateTime(2017, 1, 1);
        var secondDate = new DateTime(2017, 12, 12);
        var queryName = "PlayStation%";

        var products2017 = FindData(productInventoryDbContext, queryName, firstDate, secondDate);

        Console.WriteLine($"************* Number of found Records: {products2017.Count()} !******************");

        //Console.WriteLine("******************  Clear Query Plans! ******************");
        //ClearQueryPlans(productInventoryDbContext);

        Console.WriteLine("************* Find Big amount of Data 2018!******************");

        firstDate = new DateTime(2018, 1, 1);
        secondDate = new DateTime(2018, 12, 12);
        queryName = "XBox%";

        var products2018 = FindData(productInventoryDbContext, queryName, firstDate, secondDate);

        Console.WriteLine($"************* Number of found Records: {products2018.Count()} !******************");
      }
    }

    /// <summary>Finds the first big data amount then the small data amount.</summary>
    private static void FindFirstBigDataAmountThenSmallDataAmount()
    {

      using (var productInventoryDbContext = new ProductInventoryDbContext())
      {
        // Command listener to add "Recompile" to the query.
        var listener = productInventoryDbContext.GetService<DiagnosticSource>();
        (listener as DiagnosticListener).SubscribeWithAdapter(new QueryCommandListener());

        Console.WriteLine("******************  Clear Query Plans! ******************");
        //ClearQueryPlans(productInventoryDbContext);

        Console.WriteLine("************* Find Big amount of Data 2018! ******************");
        var firstDate = new DateTime(2018, 1, 1);
        var secondDate = new DateTime(2018, 12, 12);
        var queryName = "XBox%";

        var products2018 = FindData(productInventoryDbContext, queryName, firstDate, secondDate);

        Console.WriteLine($"************* Number of found Records: {products2018.Count()} !******************");

        Console.WriteLine("************* Find small amount of Data 2017! ******************");

        firstDate = new DateTime(2017, 1, 1);
        secondDate = new DateTime(2017, 12, 12);
        queryName = "PlayStation%";

        var products2017 = FindData(productInventoryDbContext, queryName, firstDate, secondDate);

        Console.WriteLine($"************* Number of found Records: {products2017.Count()} !******************");
      }
    }

    /// <summary>
    /// Finds the data.
    /// </summary>
    /// <param name="productInventoryDbContext">The product inventory database context.</param>
    /// <param name="queryName">Name of the query.</param>
    /// <param name="firstDate">The first date.</param>
    /// <param name="secondDate">The second date.</param>
    /// <returns>List of founded Products.</returns>
    private static IEnumerable<ProductInventory> FindData(ProductInventoryDbContext productInventoryDbContext, string queryName, DateTime firstDate, DateTime secondDate)
    {
      return productInventoryDbContext.ProductInventories
        .Where(x => EF.Functions.Like(x.Name, queryName) && x.CreateDate >= firstDate && x.CreateDate <= secondDate)
        .OrderByDescending(x => x.CreateDate)
        .ToList();
    }

    /// <summary>Clears the query plans.</summary>
    /// <param name="productInventoryDbContext">The product inventory database context.</param>
    private static void ClearQueryPlans(ProductInventoryDbContext productInventoryDbContext)
    {
      var clearQuery =
        @"ALTER DATABASE ProductInventoryDatabase SET QUERY_STORE CLEAR; 

          DECLARE @id int  
            DECLARE adhoc_queries_cursor CURSOR   
          FOR   
            SELECT q.query_id  
            FROM sys.query_store_query_text AS qt  
            JOIN sys.query_store_query AS q   
            ON q.query_text_id = qt.query_text_id  
          JOIN sys.query_store_plan AS p   
            ON p.query_id = q.query_id  
          JOIN sys.query_store_runtime_stats AS rs   
            ON rs.plan_id = p.plan_id  
          GROUP BY q.query_id  
            HAVING SUM(rs.count_executions) < 2   
          AND MAX(rs.last_execution_time) < DATEADD (hour, -24, GETUTCDATE())  
          ORDER BY q.query_id ;  
      
          OPEN adhoc_queries_cursor ;  
          FETCH NEXT FROM adhoc_queries_cursor INTO @id;  
          WHILE @@fetch_status = 0  
          BEGIN   
            PRINT @id  
            EXEC sp_query_store_remove_query @id  
          FETCH NEXT FROM adhoc_queries_cursor INTO @id  
          END   
          CLOSE adhoc_queries_cursor ;  
          DEALLOCATE adhoc_queries_cursor;
      ";

      productInventoryDbContext.Database.ExecuteSqlCommand(clearQuery);
    }
  }
}