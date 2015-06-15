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
}