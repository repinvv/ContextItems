namespace ContextItems.OracleContextItemsGeneration.StaticFiles
{
    using System;
    using System.Collections.Generic;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.StaticFiles;

    internal class OracleEntityInterfaceGenerator : IFileGenerator
    {
        public string GetName()
        {
            return "IOracleEntity";
        }

        public string FileContents(string outNamespace, IEnumerable<DbModel> models)
        {
            return "namespace " + outNamespace + Environment.NewLine +
@"{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Oracle.DataAccess.Client;

    public interface IOracleEntity
    {
        void Materialize(IDataRecord input, IFieldsCaster caster);

        string GetOracleInsertRequest(string schemaName);

        string GetOracleUpdateRequest(string schemaName);

        string GetOracleDeleteRequest(string schemaName);

        OracleParameter AddOracleInsertParameters(IEnumerable<IOracleEntity> entities, OracleCommand command);

        void AddOracleUpdateParameters(IEnumerable<IOracleEntity> entities, OracleCommand command);

        void AddOracleDeleteParameters(IEnumerable<IOracleEntity> entities, OracleCommand command);

        void ApplyOutParameter<T>(OracleParameter outParameter, List<T> array) where T : class, IOracleEntity;

        Type[] GetFieldTypes();
    }
}";
        }
    }
}
