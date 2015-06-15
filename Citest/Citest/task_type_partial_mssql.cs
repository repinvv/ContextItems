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
    using System.Linq;
    using System.Text;

    public partial class task_type : IMsSqlEntity
    {
        public static string SequenceName = "task_type_seq";
        public override bool Equals(object obj)
        {
            return Equals(obj as task_type);
        }

        public bool Equals(task_type other)
        {
            if (other == null)
            {
                return false;
            }

            return task_type_id == other.task_type_id;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return task_type_id.GetHashCode();
            }
        }

        void IMsSqlEntity.Materialize(IDataRecord input)
        {
            task_type_id = input.GetInt32(0);
            task_name = input[1] as string;
            assembly_location = input[2] as string;
            class_name = input[3] as string;
        }

        bool IMsSqlEntity.HasIdentity()
        {
            return true;
        }

        int IMsSqlEntity.GetNumberOfFields()
        {
            return 4;
        }

        string IMsSqlEntity.GetTableName()
        {
            return "dbo.task_type";
        }

        string IMsSqlEntity.GetMsSqlInsertRequest()
        {
            return "INSERT INTO dbo.task_type\r\n" +
                "  (task_name, assembly_location, class_name)\r\n" +
                "OUTPUT inserted.task_type_id\r\n" +
                "VALUES\r\n";
        }

        string IMsSqlEntity.GetMsSqlInsertValues(int i)
        {
            return "(@parm1i" + i + ", @parm2i" + i + ", @parm3i" + i + ")";
        }

        IEnumerable<SqlParameter> IMsSqlEntity.GetMsSqlInsertParameters(int index)
        {
            yield return new SqlParameter("parm1i" + index, task_name ?? (object)DBNull.Value);
            yield return new SqlParameter("parm2i" + index, assembly_location ?? (object)DBNull.Value);
            yield return new SqlParameter("parm3i" + index, class_name ?? (object)DBNull.Value);
        }

        void IMsSqlEntity.PopulateIdentity(IDataRecord input)
        {
            task_type_id = (int)input["task_type_id"];
        }

        object IMsSqlEntity.GetValue(int i)
        {
            switch (i)
            {
                case 0:
                    return task_type_id;
                case 1:
                    return task_name;
                case 2:
                    return assembly_location;
                case 3:
                    return class_name;
            }

            return null;
        }

        string IMsSqlEntity.GetSequenceName()
        {
            return SequenceName;
        }

        void IMsSqlEntity.SetSequence<T>(List<T> list, object seq)
        {
            var sequence = (int)seq;
            list.ForEach(x => (x as task_type).task_type_id = sequence++);
        }

        string IMsSqlEntity.GetUpdateRequest(int index)
        {
            return "UPDATE dbo.task_type\r\n" +
                "SET task_name = @parm1i" + index + ",\r\n" +
                "    assembly_location = @parm2i" + index + ",\r\n" +
                "    class_name = @parm3i" + index + "\r\n" +
                "WHERE task_type_id = @parm4i" + index + ";";
        }

        IEnumerable<SqlParameter> IMsSqlEntity.GetUpdateParameters(int index)
        {
            yield return new SqlParameter("parm1i" + index, task_name ?? (object)DBNull.Value);
            yield return new SqlParameter("parm2i" + index, assembly_location ?? (object)DBNull.Value);
            yield return new SqlParameter("parm3i" + index, class_name ?? (object)DBNull.Value);
            yield return new SqlParameter("parm4i" + index, task_type_id);
        }

        string IMsSqlEntity.GetBulkUpdateRequest(string table)
        {
            return "UPDATE dbo.task_type\r\n" +
                "SET task_name = s.task_name,\r\n" +
                "    assembly_location = s.assembly_location,\r\n" +
                "    class_name = s.class_name\r\n" +
                "FROM dbo.task_type tj \r\n" +
                "INNER JOIN " + table + " s\r\n" +
                "ON  tj.task_type_id = s.task_type_id\r\n";
        }

        string IMsSqlEntity.GetCreateTempTableRequest(string table)
        {
            return "CREATE TABLE " + table + "\r\n" +
                "(\r\n" +
                "task_type_id int,\r\n" +
                "task_name nvarchar,\r\n" +
                "assembly_location nvarchar,\r\n" +
                "class_name nvarchar,\r\n" +
                ")";
        }

        int IMsSqlEntity.GetNumberOfKeyFields()
        {
             return 1;
        }

        string IMsSqlEntity.GetDeleteRequest(int index)
        {
            return "DELETE FROM dbo.task_type\r\n" +
                "WHERE task_type_id = @parm1i" + index + ";";
        }

        IEnumerable<SqlParameter> IMsSqlEntity.GetDeleteParameters(int index)
        {
            yield return new SqlParameter("parm1i" + index, task_type_id);
        }

        string IMsSqlEntity.GetBulkDeleteRequest(string table)
        {
            return "DELETE t FROM dbo.task_type t\r\n" +
                "INNER JOIN " + table + " s\r\n" +
                "ON  t.task_type_id = s.task_type_id\r\n";
        }

        string IMsSqlEntity.GetCreateIdTableRequest(string table)
        {
            return "CREATE TABLE " + table + "\r\n" +
                "(\r\n" +
                "task_type_id int,\r\n" +
                ")";
        }

        object IMsSqlEntity.GetKeyValue(int i)
        {
            switch (i)
            {
                case 0:
                    return task_type_id;
            }

            return null;
        }
    }
}
