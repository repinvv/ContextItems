namespace ContextItems.CommonGeneration.ContextItems.StaticFiles
{
    using System;
    using System.Collections.Generic;

    public class ConnectionHandlerGenerator : IFileGenerator
    {
        public string GetName()
        {
            return "ConnectionHandler";
        }

        public string FileContents(string outputNamespace, IEnumerable<DbModel> models)
        {
            return "namespace " + outputNamespace + Environment.NewLine +
@"{
    using System;
    using System.Data;
    using System.Data.Common;

    public class ConnectionHandler : IDisposable
    {
        private readonly DbConnection connection;

        public ConnectionHandler(DbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                this.connection = connection;
            }
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
    }
}";
        }
    }
}
