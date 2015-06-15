//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Citest
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;

    internal class AdoCommands : IAdoCommands
    {
        const SqlBulkCopyOptions BulkOptions = SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.KeepNulls | SqlBulkCopyOptions.KeepIdentity;

        public void ExecuteCommand(string request, IEnumerable<object> parameters, DbConnection connection, DbTransaction transaction = null)
        {
            using (new ConnectionHandler(connection))
            {
                int count = 0;
                var sqlParameters = parameters.Select(x => new SqlParameter((count++).ToString(), x)).ToArray();
                RunCommand(request, sqlParameters, connection, transaction);
            }
        }

        public void RunCommand(string request, SqlParameter[] parameters, DbConnection connection, DbTransaction transaction)
        {
            using (var command = new SqlCommand(request, (SqlConnection)connection))
            {
                command.Transaction = (SqlTransaction)transaction;
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
        }

        public T ExecuteScalar<T>(string request, IEnumerable<object> parameters, DbConnection connection, DbTransaction transaction = null)
        {
            using (new ConnectionHandler(connection))
            {
                using (var command = new SqlCommand(request, (SqlConnection)connection))
                {
                    command.Transaction = (SqlTransaction)transaction;
                    int count = 0;
                    command.Parameters.AddRange(parameters.Select(x => new SqlParameter((count++).ToString(), x)).ToArray());
                    return (T)command.ExecuteScalar();
                }
            }
        }

        public List<T> ExecuteSelect<T>(string request, IEnumerable<object> parameters, DbConnection connection, DbTransaction transaction = null)
        {
            var func = Material.GetFunc(typeof(T)) as Func<IDataRecord, int, T> ?? ((record, index) => (T)record.GetValue(index));
            return ExecuteSelect(request, parameters, reader => func(reader, 0), connection, transaction);
        }

        public List<T> ExecuteSelect<T>(string request, IEnumerable<object> parameters, Func<IDataReader, T> func, DbConnection connection, DbTransaction transaction = null)
        {
            var output = new List<T>();
            using (new ConnectionHandler(connection))
            {
                using (var command = new SqlCommand(request, (SqlConnection)connection))
                {
                    command.Transaction = (SqlTransaction)transaction;
                    int count = 0;
                    command.Parameters.AddRange(parameters.Select(x => new SqlParameter((count++).ToString(), x)).ToArray());
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

        public void SplitRun<T>(List<T> list, Action<List<T>> action, int batchSize = 1000)
        {
            var batchCount = (list.Count / batchSize) + (list.Count % batchSize == 0 ? 0 : 1);
            var outputSize = (list.Count / batchCount) + (list.Count % batchCount == 0 ? 0 : 1);
            for (int i = 0; i < list.Count; i += outputSize)
            {
                action(list.GetRange(i, Math.Min(outputSize, list.Count - i)));
            }
        }

        public void RunCommandPopulateIdentity(string request, SqlParameter[] parameters, List<IMsSqlEntity> entities, DbConnection connection, DbTransaction transaction)
        {
            using (var command = new SqlCommand(request, (SqlConnection)connection))
            {
                command.Transaction = (SqlTransaction)transaction;
                command.Parameters.AddRange(parameters);
                int i = 0;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entities[i++].PopulateIdentity(reader);
                    }
                }
            }
        }

        public void BulkInsert<T>(List<T> list, string tableName, DbConnection connection, DbTransaction transaction) where T : class, IMsSqlEntity
        {
            using (var bulk = new SqlBulkCopy((SqlConnection)connection, BulkOptions, (SqlTransaction)transaction))
            {
                bulk.DestinationTableName = tableName;
                bulk.WriteToServer(new BulkInsertDataReader<T>(list));
            }
        }

        public void BulkInsertKeys<T>(List<T> list, string tableName, DbConnection connection, DbTransaction transaction) where T : class, IMsSqlEntity
        {
            using (var bulk = new SqlBulkCopy((SqlConnection)connection, BulkOptions, (SqlTransaction)transaction))
            {
                bulk.DestinationTableName = tableName;
                bulk.WriteToServer(new BulkInsertKeysDataReader<T>(list));
            }
        }

        public string CreateTempTableName()
        {
            return "#a" + Guid.NewGuid().ToString("N");
        }

        public string GetDropRequest(string table)
        {
            return "drop table " + table;
        }
    }
}