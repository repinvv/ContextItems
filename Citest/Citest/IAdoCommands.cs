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

    public interface IAdoCommands
    {
        void ExecuteCommand(string request, IEnumerable<object> parameters, DbConnection connection, DbTransaction transaction = null);

        T ExecuteScalar<T>(string request, IEnumerable<object> parameters, DbConnection connection, DbTransaction transaction = null);

        List<T> ExecuteSelect<T>(string request, IEnumerable<object> parameters, DbConnection connection, DbTransaction transaction = null);

        List<T> ExecuteSelect<T>(string request, IEnumerable<object> parameters, Func<IDataReader, T> func, DbConnection connection, DbTransaction transaction = null);
    }
}