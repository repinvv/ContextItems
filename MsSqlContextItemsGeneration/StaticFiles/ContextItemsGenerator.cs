namespace ContextItems.MsSqlContextItemsGeneration.StaticFiles
{
    using System;
    using System.Collections.Generic;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.StaticFiles;

    internal class ContextItemsGenerator : IFileGenerator
    {
        public string GetName()
        {
            return "ContextItems";
        }

        public string FileContents(string outputNamespace, IEnumerable<DbModel> models)
        {
            return "namespace " + outputNamespace + Environment.NewLine +
@"{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using EntityFramework.Extensions;

    public class MsSqlContextItems
    {
        private const int MaxParms = 750;
        private const int MaxItems = 150;
        private int requestCacheLength;
        private string requestCache;

        readonly AdoCommands commands = new AdoCommands();

        public IAdoCommands Commands { get { return commands; } }

        public List<T> Materialize<T>(IQueryable<T> source, DbConnection connection, DbTransaction transaction = null) where T : class, IMsSqlEntity, new()
        {
            var objectQuery = source.ToObjectQuery();
            var parameters = objectQuery.Parameters.Select(x => new SqlParameter(x.Name, x.Value));
            var request = objectQuery.ToTraceString();
            Func<IDataReader, T> func = reader =>
                {
                    var output = new T();
                    output.Materialize(reader);
                    return output;
                };

            return Commands.ExecuteSelect<T>(request, parameters, func, connection, transaction);
        }

        private void RangeInsert(List<IMsSqlEntity> entities, DbConnection connection, DbTransaction transaction)
        {
            int i;
            if (entities.Count != requestCacheLength)
            {
                var sb = new StringBuilder();
                sb.AppendLine(entities[0].GetMsSqlInsertRequest());
                sb.AppendLine(""  "" + entities[0].GetMsSqlInsertValues(0));

                for (i = 1; i < entities.Count; i++)
                {
                    sb.AppendLine("" ,"" + entities[0].GetMsSqlInsertValues(i));
                }

                requestCache = sb.ToString();
                requestCacheLength = entities.Count;
            }

            i = 0;
            var parameters = entities.SelectMany(x => x.GetMsSqlInsertParameters(i++)).ToArray();
            commands.RunCommandPopulateIdentity(requestCache, parameters, entities, connection, transaction);
        }

        public void RangeInsert<T>(IEnumerable<T> entities, DbConnection connection, DbTransaction transaction) where T : class, IMsSqlEntity
        {
            var list = entities as List<T> ?? entities.ToList();
            if (!list.Any())
            {
                return;
            }

            using (new ConnectionHandler(connection))
            {
                if (list[0].HasIdentity())
                {
                    var parmCount = list[0].GetNumberOfFields() - 1;
                    var maxItems = Math.Min(MaxItems, MaxParms / parmCount);
                    requestCacheLength = 0;
                    commands.SplitRun(list.Cast<IMsSqlEntity>().ToList(), x => RangeInsert(x, connection, transaction), maxItems);
                }
                else
                {
                    commands.BulkInsert(list, list[0].GetTableName(), connection, transaction);
                }
            }
        }

        public void RangeInsertSequenced<T>(IEnumerable<T> entities, DbConnection connection, DbTransaction transaction = null) where T : class, IMsSqlEntity
        {
            var list = entities as List<T> ?? entities.ToList();
            if (!list.Any())
            {
                return;
            }

            using (new ConnectionHandler(connection))
            {
                using (var cmd = new SqlCommand(""sys.sp_sequence_get_range"", (SqlConnection)connection))
                {                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = transaction as SqlTransaction;
                    cmd.Parameters.AddWithValue(""@sequence_name"", list[0].GetSequenceName());
                    cmd.Parameters.AddWithValue(""@range_size"", list.Count);
                    SqlParameter firstValue = new SqlParameter(""@range_first_value"", SqlDbType.Variant);
                    firstValue.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(firstValue);
                    cmd.ExecuteNonQuery();
                    list[0].SetSequence(list, firstValue.Value);
                }

                commands.BulkInsert(list, list[0].GetTableName(), connection, transaction);
            }
        }

        private void SimpleRangeUpdate<T>(List<T> entities, DbConnection connection, DbTransaction transaction) where T : class, IMsSqlEntity
        {
            int i = 1;
            var request = string.Join(Environment.NewLine, entities.Select(x => x.GetUpdateRequest(i++)));
            i = 1;
            var parameters = entities.SelectMany(x => x.GetUpdateParameters(i++)).ToArray();
            commands.RunCommand(request, parameters, connection, transaction);
        }

        public void RangeUpdate<T>(IEnumerable<T> entities, DbConnection connection, DbTransaction transaction) where T : class, IMsSqlEntity
        {
            var list = entities as List<T> ?? entities.ToList();
            if (!list.Any())
            {
                return;
            }

            using (new ConnectionHandler(connection))
            {
                if (list.Count <= MaxItems && list.Count <= MaxParms / list[0].GetNumberOfFields())
                {
                    SimpleRangeUpdate(list, connection, transaction);
                    return;
                }

                var table = commands.CreateTempTableName();
                commands.RunCommand(list[0].GetCreateTempTableRequest(table), new SqlParameter[0], connection, transaction);
                commands.BulkInsert(list, table, connection, transaction);
                commands.RunCommand(list[0].GetBulkUpdateRequest(table), new SqlParameter[0], connection, transaction);
                commands.RunCommand(commands.GetDropRequest(table), new SqlParameter[0], connection, transaction);
            }
        }

        private void RangeDelete(List<IMsSqlEntity> entities, DbConnection connection, DbTransaction transaction)
        {
            int i = 1;
            var request = string.Join(Environment.NewLine, entities.Select(x => x.GetDeleteRequest(i++)));
            i = 1;
            var parameters = entities.SelectMany(x => x.GetDeleteParameters(i++)).ToArray();
            commands.RunCommand(request, parameters, connection, transaction);
        }

        public void RangeDelete<T>(IEnumerable<T> entities, DbConnection connection, DbTransaction transaction = null) where T : class, IMsSqlEntity
        {
            var list = entities as List<T> ?? entities.ToList();
            if (!list.Any())
            {
                return;
            }

            using (new ConnectionHandler(connection))
            {
                if (list.Count <= MaxItems && list.Count <= MaxParms / list[0].GetNumberOfKeyFields())
                {
                    RangeDelete(list, connection, transaction);
                    return;
                }

                var table = commands.CreateTempTableName();
                commands.RunCommand(list[0].GetCreateIdTableRequest(table), new SqlParameter[0], connection, transaction);
                commands.BulkInsertKeys(list, table, connection, transaction);
                commands.RunCommand(list[0].GetBulkDeleteRequest(table), new SqlParameter[0], connection, transaction);
                commands.RunCommand(commands.GetDropRequest(table), new SqlParameter[0], connection, transaction);
            }
        }
    }
}";
        }
    }
}
