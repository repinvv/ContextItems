namespace ContextItems.OracleContextItemsGeneration.StaticFiles
{
    using System;
    using System.Collections.Generic;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.StaticFiles;

    internal class AdoCommandsGenerator : IFileGenerator 
    {
        public string GetName()
        {
            return "AdoCommands";
        }

        public string FileContents(string outNamespace, IEnumerable<DbModel> models)
        {
            return "namespace " + outNamespace + Environment.NewLine +
@"{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using Oracle.DataAccess.Client;

    internal class AdoCommands : IAdoCommands
    {
        public void ExecuteCommand(string request, IEnumerable<object> parameters, DbConnection connection, DbTransaction transaction = null)
        {
            using (new ConnectionHandler(connection))
            {
                using (var command = new OracleCommand(request, (OracleConnection)connection))
                {
                    command.Transaction = (OracleTransaction)transaction;
                    int count = 0;
                    command.Parameters.AddRange(parameters.Select(x => new OracleParameter((count++).ToString(), x)).ToArray());
                    command.ExecuteNonQuery();
                }
            }
        }

        public T ExecuteScalar<T>(string request, IEnumerable<object> parameters, DbConnection connection, DbTransaction transaction = null)
        {
            using (new ConnectionHandler(connection))
            {
                using (var command = new OracleCommand(request, (OracleConnection)connection))
                {
                    command.Transaction = (OracleTransaction)transaction;
                    int count = 0;
                    command.Parameters.AddRange(parameters.Select(x => new OracleParameter((count++).ToString(), x)).ToArray());
                    return (T)command.ExecuteScalar();
                }
            }
        }

        public List<T> ExecuteSelect<T>(string request, IEnumerable<object> parameters, DbConnection connection, DbTransaction transaction = null)
        {
            var func = Material.GetFunc(typeof(T)) as Func<IDataRecord, int, T> ?? ((record, index) => (T)record.GetValue(index));
            return ExecuteSelect<T>(request, parameters, reader => func(reader, 0), connection, transaction);
        }

        public List<T> ExecuteSelect<T>(string request, IEnumerable<object> parameters, Func<IDataReader, T> func, DbConnection connection,DbTransaction transaction = null)
        {
            var output = new List<T>();
            using (new ConnectionHandler(connection))
            {
                using (var command = new OracleCommand(request, (OracleConnection)connection))
                {
                    command.Transaction = (OracleTransaction)transaction;
                    int count = 0;
                    command.Parameters.AddRange(parameters.Select(x => new OracleParameter((count++).ToString(), x)).ToArray());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            output.Add(func(reader));
                        }
                    }
                }
            }

            return output;
        }
    }
}";
        }
    }
}
