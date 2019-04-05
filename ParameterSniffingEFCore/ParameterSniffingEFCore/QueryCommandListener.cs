using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DiagnosticAdapter;

namespace ParameterSniffingEFCore
{
  public class QueryCommandListener
  {
    [DiagnosticName("Microsoft.EntityFrameworkCore.Database.Command.CommandExecuting")]
    public void OnCommandExecuting(DbCommand command, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, bool async, DateTimeOffset startTime)
    {
      if (command.CommandType != CommandType.Text || !(command is SqlCommand))
        return;

      if (command.CommandText.Contains("ProductInventories") && !command.CommandText.Contains("option(recompile)"))
      {
        command.CommandText = command.CommandText + " option(recompile)";
      }
   
    }

    [DiagnosticName("Microsoft.EntityFrameworkCore.Database.Command.CommandExecuted")]
    public void OnCommandExecuted(object result, bool async)
    {
    }

    [DiagnosticName("Microsoft.EntityFrameworkCore.Database.Command.CommandError")]
    public void OnCommandError(Exception exception, bool async)
    {
    }
  }
}