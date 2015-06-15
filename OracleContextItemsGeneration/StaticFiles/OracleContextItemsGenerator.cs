namespace ContextItems.OracleContextItemsGeneration.StaticFiles
{
    using System;
    using System.Collections.Generic;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.StaticFiles;

    internal class OracleContextItemsGenerator : IFileGenerator
    {
        public string GetName()
        {
            return "ContextItems";
        }

        public string FileContents(string outNamespace, IEnumerable<DbModel> models)
        {
            return "namespace " + outNamespace + Environment.NewLine +
@"{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using EntityFramework.Extensions;
    using Oracle.DataAccess.Client;

    public class OracleContextItems
    {
        private readonly IAdoCommands adoCommands = new AdoCommands();

        private readonly string schemaName;

        public OracleContextItems(string schemaName)
        {
            this.schemaName = string.IsNullOrWhiteSpace(schemaName) ? string.Empty : schemaName + ""."";
        }

        public OracleContextItems()
        {
            this.schemaName = string.Empty;
        }

        public IAdoCommands Commands { get { return adoCommands; } }

        public IDisposable GetConnectionHandler(DbConnection connection)
        {
            return new ConnectionHandler(connection);
        }

        public void RangeInsert<T>(IEnumerable<T> entities, DbConnection connection, DbTransaction transaction = null) where T : class, IOracleEntity
        {
            var list = entities as List<T> ?? entities.ToList();
            if (!list.Any())
            {
                return;
            }

            var request = list[0].GetOracleInsertRequest(schemaName);
            using (new ConnectionHandler(connection))
            {
                using (var command = new OracleCommand(request, (OracleConnection)connection))
                {
                    command.Transaction = (OracleTransaction)transaction;
                    var outParameter = list[0].AddOracleInsertParameters(list, command);
                    command.ExecuteNonQuery();
                    list[0].ApplyOutParameter(outParameter, list);
                }
            }
        }

        public void RangeUpdate<T>(IEnumerable<T> entities, DbConnection connection, DbTransaction transaction = null) where T : class, IOracleEntity
        {
            var list = entities as List<T> ?? entities.ToList();
            if (!list.Any())
            {
                return;
            }

            var request = list[0].GetOracleUpdateRequest(schemaName);
            using (new ConnectionHandler(connection))
            {
                using (var command = new OracleCommand(request, (OracleConnection)connection))
                {
                    command.Transaction = (OracleTransaction)transaction;
                    list[0].AddOracleUpdateParameters(list, command);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RangeDelete<T>(IEnumerable<T> entities, DbConnection connection, DbTransaction transaction = null) where T : class, IOracleEntity
        {
            var list = entities as List<T> ?? entities.ToList();
            if (!list.Any())
            {
                return;
            }

            var request = list[0].GetOracleDeleteRequest(schemaName);
            using (new ConnectionHandler(connection))
            {
                using (var command = new OracleCommand(request, (OracleConnection)connection))
                {
                    command.Transaction = (OracleTransaction)transaction;
                    list[0].AddOracleDeleteParameters(list, command);
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<T> Materialize<T>(IQueryable<T> query, DbConnection connection, DbTransaction transaction = null) where T : class, IOracleEntity, new()
        {
            var objectQuery = query.ToObjectQuery();
            var parameters = objectQuery.Parameters.Select(x => new OracleParameter(x.Name, x.Value)).ToArray();
            var request = objectQuery.ToTraceString();

            using (new ConnectionHandler(connection))
            {
                using (var command = new OracleCommand(request, (OracleConnection)connection))
                {
                    command.Transaction = (OracleTransaction)transaction;
                    command.Parameters.AddRange(parameters);
                    using (var reader = command.ExecuteReader())
                    {
                        return new Materializer<T>(reader).GetEntities();
                    }
                }
            }
        }
    }
}";
        }
    }
}
