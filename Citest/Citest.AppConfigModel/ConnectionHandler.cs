//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Citest.AppConfigModel
{
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
}