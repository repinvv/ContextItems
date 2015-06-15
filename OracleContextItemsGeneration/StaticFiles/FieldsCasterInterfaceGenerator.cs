namespace ContextItems.OracleContextItemsGeneration.StaticFiles
{
    using System;
    using System.Collections.Generic;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.StaticFiles;

    internal class FieldsCasterInterfaceGenerator : IFileGenerator 
    {
        public string GetName()
        {
            return "IFieldsCaster";
        }

        public string FileContents(string outNamespace, IEnumerable<DbModel> models)
        {
            return "namespace " + outNamespace + Environment.NewLine +
@"{
    using System.Data;

    public interface IFieldsCaster
    {
        T GetRecord<T>(int index, IDataRecord record);

        T? GetNullableRecord<T>(int index, IDataRecord record) where T : struct;
    }
}";
        }
    }
}
