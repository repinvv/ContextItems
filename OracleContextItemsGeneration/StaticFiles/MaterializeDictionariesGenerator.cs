namespace ContextItems.OracleContextItemsGeneration.StaticFiles
{
    using System;
    using System.Collections.Generic;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.StaticFiles;

    internal class MaterializeDictionariesGenerator : IFileGenerator
    {
        public string GetName()
        {
            return "Material";
        }

        public string FileContents(string outNamespace, IEnumerable<DbModel> models)
        {
            return "namespace " + outNamespace + Environment.NewLine +
@"{
    using System;
    using System.Collections;
    using System.Data;

    internal static class Material
    {
        private static readonly Hashtable Funcs = new Hashtable();

        static Material()
        {
            Func<IDataRecord, int, char> func1 = (record, index) => record.GetChar(index);
            AddFunc(func1, typeof(char));
            Func<IDataRecord, int, string> func2 = (record, index) => record.GetString(index);
            AddFunc(func2, typeof(string));
            Func<IDataRecord, int, byte> func3 = (record, index) => record.GetByte(index);
            AddFunc(func3, typeof(byte));
            Func<IDataRecord, int, short> func4 = (record, index) => record.GetInt16(index);
            AddFunc(func4, typeof(short));
            Func<IDataRecord, int, int> func5 = (record, index) => record.GetInt32(index);
            AddFunc(func5, typeof(int));
            Func<IDataRecord, int, long> func6 = (record, index) => record.GetInt64(index);
            AddFunc(func6, typeof(long));
            Func<IDataRecord, int, decimal> func7 = (record, index) => record.GetDecimal(index);
            AddFunc(func7, typeof(decimal));
            Func<IDataRecord, int, float> func8 = (record, index) => record.GetFloat(index);
            AddFunc(func8, typeof(float));
            Func<IDataRecord, int, double> func9 = (record, index) => record.GetDouble(index);
            AddFunc(func9, typeof(double));
            Func<IDataRecord, int, DateTime> func10 = (record, index) => record.GetDateTime(index);
            AddFunc(func10, typeof(DateTime));
            Func<IDataRecord, int, bool> func11 = (record, index) => record.GetBoolean(index);
            AddFunc(func11, typeof(bool));

            // deviate funcs
            func7 = (record, index) => (decimal)record.GetDouble(index);
            AddFunc(func7, typeof(decimal), typeof(double));

            func5 = (record, index) => (int)record.GetInt64(index);
            AddFunc(func5, typeof(int), typeof(long));
        }

        private static void AddFunc(object func, Type returnType, Type recordType = null)
        {
            Funcs.Add(new { returnType, recordType = recordType ?? returnType }, func);
        }

        public static object GetFunc(Type returnType, Type recordType = null)
        {
            return Funcs[(new { returnType, recordType = recordType ?? returnType })];
        }
    }
}";
        }
    }
}
