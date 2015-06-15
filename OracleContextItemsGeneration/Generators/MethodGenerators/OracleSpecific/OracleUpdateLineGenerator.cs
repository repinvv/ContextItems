namespace ContextItems.OracleContextItemsGeneration.Generators.MethodGenerators.OracleSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class OracleUpdateLineGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("string IOracleEntity.GetOracleUpdateRequest(string schemaName)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            var nonKeyFields = model.Fields.Except<DbModelField>(model.KeyFields).ToArray();
            if (!model.KeyFields.Any() || !nonKeyFields.Any())
            {
                stringGenerator.AppendLine("return string.Empty;");
                return;
            }

            stringGenerator.AppendLine("return \"UPDATE \" + schemaName + \"" + model.Name + "\\r\\n\"");
            stringGenerator.PushIndent();

            int i = 1;
            for (int index = 0; index < nonKeyFields.Length; index++)
            {
                var field = nonKeyFields[index];
                var startline = index == 0 ? "SET " : "    ";
                var endline = index == nonKeyFields.Length - 1 ? string.Empty : ",";
                stringGenerator.AppendLine("+ \"" + startline + field.FieldName + " = :parm" + i++ + endline + "\\r\\n\"");
            }

            for (int index = 0; index < model.KeyFields.Length; index++)
            {
                var field = model.KeyFields[index];
                var startline = index == 0 ? "WHERE " : "  AND ";
                var endline = index == model.KeyFields.Length - 1 ? ";" : string.Empty;
                stringGenerator.AppendLine("+ \"" + startline + field.FieldName + " = :parm" + i++ + "\\r\\n\"" + endline);
            }

            stringGenerator.PopIndent();
        }
    }
}
