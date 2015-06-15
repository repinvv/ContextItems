namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class GetDeleteRequestGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("string IMsSqlEntity.GetDeleteRequest(int index)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("return \"DELETE FROM " + model.SchemaName + model.Name + "\\r\\n\" +");
            stringGenerator.PushIndent();
            var fields = model.KeyFields.Any<DbModelField>() ? model.KeyFields : model.Fields;
            int n = 1;
            for (int i = 0; i < fields.Length; i++)
            {
                var linestart = i == 0 ? "WHERE " : "  AND ";
                var lineend = i == fields.Length - 1 ? ";" : " +";
                var inlineend = i == fields.Length - 1 ? ";" : ",";
                stringGenerator.AppendLine("\"" + linestart + fields[i].FieldName + " = @parm" + n++ + "i\" + index + \"" + inlineend + "\"" + lineend);
            }

            stringGenerator.PopIndent();
        }
    }
}
