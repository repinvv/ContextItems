namespace ContextItems.MsSqlContextItemsGeneration.StaticFiles
{
    using System;
    using System.Collections.Generic;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.StaticFiles;

    internal class MsSqlEntityGenerator : IFileGenerator
    {
        public string GetName()
        {
            return "IMsSqlEntity";
        }

        public string FileContents(string outputNamespace, IEnumerable<DbModel> models)
        {
            return "namespace " + outputNamespace + Environment.NewLine +
@"{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    public interface IMsSqlEntity
    {
        // materialize
        void Materialize(IDataRecord input);

        // insert
        int GetNumberOfFields();

        string GetTableName();

        bool HasIdentity();

        string GetMsSqlInsertRequest();

        string GetMsSqlInsertValues(int index);

        IEnumerable<SqlParameter> GetMsSqlInsertParameters(int index);

        void PopulateIdentity(IDataRecord input);

        object GetValue(int i);

        string GetSequenceName();

        void SetSequence<T>(List<T> list, object seq) where T : class, IMsSqlEntity;

        // update
        string GetUpdateRequest(int index);

        IEnumerable<SqlParameter> GetUpdateParameters(int index);

        string GetCreateTempTableRequest(string table);

        string GetBulkUpdateRequest(string table);

        // delete
        int GetNumberOfKeyFields();

        string GetDeleteRequest(int index);

        IEnumerable<SqlParameter> GetDeleteParameters(int index);

        string GetCreateIdTableRequest(string table);

        string GetBulkDeleteRequest(string table);

        object GetKeyValue(int i);
    }
}";
        }
    }
}
