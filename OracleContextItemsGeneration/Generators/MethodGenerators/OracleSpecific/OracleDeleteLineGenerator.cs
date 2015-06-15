namespace ContextItems.OracleContextItemsGeneration.Generators.MethodGenerators.OracleSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class OracleDeleteLineGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("string IOracleEntity.GetOracleDeleteRequest(string schemaName)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            var fields = model.KeyFields.Any<DbModelField>() ? model.KeyFields : model.Fields;

            stringGenerator.AppendLine("return \"DELETE \" + schemaName + \"" + model.Name + "\\r\\n\"");
            stringGenerator.PushIndent();

            int i = 1;
            for (int index = 0; index < fields.Length; index++)
            {
                var field = fields[index];
                var startline = index == 0 ? "WHERE " : "  AND ";
                var endline = index == model.KeyFields.Length - 1 ? ";" : string.Empty;
                stringGenerator.AppendLine("+ \"" + startline + field.FieldName + " = :parm" + i++ + "\\r\\n\"" + endline);
            }

            stringGenerator.PopIndent();
        }
    }
}
