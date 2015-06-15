namespace ContextItems.MsSqlContextItemsGeneration.StaticFiles
{
    using System;
    using System.Collections.Generic;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.StaticFiles;

    internal class BulkInsertKeysDataReaderGenerator : IFileGenerator
    {
        public string GetName()
        {
            return "BulkInsertKeysDataReader";
        }

        public string FileContents(string outputNamespace, IEnumerable<DbModel> models)
        {
            return "namespace " + outputNamespace + Environment.NewLine +
@"{
    using System.Collections.Generic;

    internal class BulkInsertKeysDataReader<T> : BulkInsertDataReader<T> where T : class, IMsSqlEntity
    {
        public BulkInsertKeysDataReader(List<T> entities) : base(entities)
        { }

        public override int FieldCount { get { return Entities[0].GetNumberOfKeyFields(); } }

        public override object GetValue(int i)
        {
            return CurrentEntity.GetKeyValue(i);
        }
    }
}";
        }
    }
}
