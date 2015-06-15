namespace ContextItems.OracleContextItemsGeneration.StaticFiles
{
    using System;
    using System.Collections.Generic;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.StaticFiles;

    internal class MaterializerGenerator : IFileGenerator
    {
        public string GetName()
        {
            return "Materializer";
        }

        public string FileContents(string outNamespace, IEnumerable<DbModel> models)
        {
            return "namespace " + outNamespace + Environment.NewLine +
@"{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    public class Materializer<T> : IFieldsCaster
        where T : class, IOracleEntity, new()
    {
        private readonly IDataReader reader;

        private readonly object[] funcs;

        public Materializer(IDataReader reader)
        {
            this.reader = reader;
            var donor = new T();
            funcs = new object[reader.FieldCount];
            var types = donor.GetFieldTypes();
            for (int i = 0; i < funcs.Length; i++)
            {
                funcs[i] = GetFunc(types[i], reader.GetFieldType(i));
            }
        }

        private object GetFunc(Type returnType, Type recordType)
        {
            return Material.GetFunc(returnType, recordType)
                   ?? GetGenericMethod(returnType, ""GetFunc"")
                       .Invoke(this, new object[0]);
        }

        private MethodInfo GetGenericMethod(Type type, string name)
        {
            var method = GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(x => x.Name == name)
                .First(x => x.IsGenericMethod);
            return method.MakeGenericMethod(new[] { type });
        }

        private Func<IDataRecord, int, T1> GetFunc<T1>()
        {
            return (record, field) => (T1)record.GetValue(field);
        }

        public List<T> GetEntities()
        {
            var list = new List<T>();
            while (reader.Read())
            {
                var entity = new T();
                entity.Materialize(reader, this);
                list.Add(entity);
            }

            return list;
        }

        public T1 GetRecord<T1>(int index, IDataRecord record)
        {
            return (funcs[index] as Func<IDataRecord, int, T1>)(record, index);
        }

        public T1? GetNullableRecord<T1>(int index, IDataRecord record) where T1 : struct
        {
            return record.IsDBNull(index) ? (T1?)null : (funcs[index] as Func<IDataRecord, int, T1>)(record, index);
        }

        public string GetString(int index, IDataRecord record)
        {
            return record.IsDBNull(index) ? null : record.GetString(index);
        }
    }
}";
        }
    }
}
