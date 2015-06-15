namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class GetBulkDeleteRequestGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("string IMsSqlEntity.GetBulkDeleteRequest(string table)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("return \"DELETE t FROM " + model.SchemaName + model.Name + " t\\r\\n\" +");
            stringGenerator.PushIndent();
            stringGenerator.AppendLine("\"INNER JOIN \" + table + \" s\\r\\n\" +");
            var fields = model.KeyFields.Any<DbModelField>() ? model.KeyFields : model.Fields;
            for (int i = 0; i < fields.Length; i++)
            {
                var linestart = i == 0 ? "ON  " : "  AND ";
                var lineend = i == fields.Length - 1 ? ";" : " +";
                stringGenerator.AppendLine("\"" + linestart + "t." + fields[i].FieldName + " = s." + fields[i].FieldName + "\\r\\n\"" + lineend);
            }

            stringGenerator.PopIndent();
        }
    }
}
