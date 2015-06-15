namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators
{
    using System;
    using System.Collections.Generic;
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class MaterializeGenerator : IMethodGenerator
    {
        private readonly Dictionary<Type, string> getters =
            new Dictionary<Type, string>
            {
                { typeof(char), "GetChar" },
                { typeof(byte), "GetByte" },
                { typeof(short), "GetInt16" },
                { typeof(int), "GetInt32" },
                { typeof(long), "GetInt64" },
                { typeof(decimal), "GetDecimal" },
                { typeof(float), "GetFloat" },
                { typeof(double), "GetDouble" },
                { typeof(DateTime), "GetDateTime" },
                { typeof(bool), "GetBoolean" },
                { typeof(Guid), "GetGuid" }
            };

        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("void IMsSqlEntity.Materialize(IDataRecord input)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            for (int index = 0; index < model.Fields.Length; index++)
            {
                var field = model.Fields[index];
                if (CommonGen.IsNullable(field.Type) || field.Type == typeof(string))
                {
                    stringGenerator.AppendLine(field.FieldName + " = input[" + index + "] as " + CommonGen.GetTypeName(field.Type) + ";");
                }
                else if (getters.ContainsKey(field.Type))
                {
                    stringGenerator.AppendLine(field.FieldName + " = input." + getters[field.Type] + "(" + index + ");");
                }
                else
                {
                    stringGenerator.AppendLine(field.FieldName + " = (" + CommonGen.GetTypeName(field.Type) + ")input[" + index + "];");
                }
            }
        }
    }
}
