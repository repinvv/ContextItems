namespace ContextItems.OracleContextItemsGeneration.Generators.MethodGenerators.OracleSpecific
{
    using System;
    using System.Collections.Generic;
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class ParametersGenerator
    {
        private Dictionary<string, string> hardTypes =
            new Dictionary<string, string>
            {
                { "timestamp", "TimeStamp" },
                { "xmltype", "XmlType" },
                { "nclob", "NClob" },
                { "nvarchar2", "NVarchar2" }
            };

        private Dictionary<Type, string> typeMap =
            new Dictionary<Type, string>
            {
                { typeof(int), "Int32" },
                { typeof(int?), "Int32" },
                { typeof(short), "Int16" },
                { typeof(short?), "Int16" },
                { typeof(long), "Int64" },
                { typeof(long?), "Int64" },
                { typeof(decimal), "Decimal" },
                { typeof(decimal?), "Decimal" }
            };

        public void GenerateParameters(DbModel model, DbModelField[] fields, IStringGenerator stringGenerator)
        {
            GenerateListsCreation(fields, stringGenerator);
            stringGenerator.AppendLine("foreach (var entity in entities.Cast<" + model.Name + ">())");
            stringGenerator.Braces(() => GenerateListsAddition(fields, stringGenerator));
            stringGenerator.AppendLine();
            stringGenerator.AppendLine("command.BindByName = true;");
            stringGenerator.AppendLine("command.ArrayBindCount = parm1.Count;");
            GenerateParametersAdd(fields, stringGenerator);
        }

        private void GenerateParametersAdd(IEnumerable<DbModelField> fields, IStringGenerator stringGenerator)
        {
            int i = 1;
            foreach (var field in fields)
            {
                var type = GetOracleDbType(field);
                stringGenerator.AppendLine("command.Parameters.Add(\"parm" + i + "\", OracleDbType." + type + ", parm" + i++ + ".ToArray(), ParameterDirection.Input);");
            }
        }

        public string GetOracleDbType(DbModelField field)
        {
            string typeName;
            if (hardTypes.TryGetValue(field.StorageType, out typeName))
            {
                return typeName;
            }

            if (field.StorageType == "number")
            {
                return typeMap[field.Type];
            }

            return char.ToUpper(field.StorageType[0]) + field.StorageType.Substring(1);
        }

        private void GenerateListsAddition(IEnumerable<DbModelField> fields, IStringGenerator stringGenerator)
        {
            int i = 1;
            foreach (var field in fields)
            {
                stringGenerator.AppendLine("parm" + i++ + ".Add(entity." + field.FieldName + ");");
            }
        }

        private void GenerateListsCreation(IEnumerable<DbModelField> fields, IStringGenerator stringGenerator)
        {
            int i = 1;
            foreach (var field in fields)
            {
                stringGenerator.AppendLine("var parm" + i++ + " = new List<" + CommonGen.GetTypeName(field.Type) + ">();");
            }
        }
    }
}
