namespace ContextItems.CommonGeneration.ContextItems.StaticFiles
{
    using System;
    using System.Collections.Generic;

    public class AdoCommandsInterfaceGenerator : IFileGenerator
    {
        public string GetName()
        {
            return "IAdoCommands";
        }

        public string FileContents(string outNamespace, IEnumerable<DbModel> models)
        {
            return "namespace " + outNamespace + Environment.NewLine +
@"{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public interface IAdoCommands
    {
        void ExecuteCommand(string request, IEnumerable<object> parameters, DbConnection connection, DbTransaction transaction = null);

        T ExecuteScalar<T>(string request, IEnumerable<object> parameters, DbConnection connection, DbTransaction transaction = null);

        List<T> ExecuteSelect<T>(string request, IEnumerable<object> parameters, DbConnection connection, DbTransaction transaction = null);

        List<T> ExecuteSelect<T>(string request, IEnumerable<object> parameters, Func<IDataReader, T> func, DbConnection connection, DbTransaction transaction = null);
    }
}";
        }
    }
}
