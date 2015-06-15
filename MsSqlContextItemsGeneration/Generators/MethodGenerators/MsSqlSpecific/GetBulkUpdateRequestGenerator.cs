namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.Parameters;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class GetBulkUpdateRequestGenerator : IMethodGenerator
    {
        private readonly IParametersService parametersService;

        public GetBulkUpdateRequestGenerator(IParametersService parametersService)
        {
            this.parametersService = parametersService;
        }

        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("string IMsSqlEntity.GetBulkUpdateRequest(string table)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            if (!model.KeyFields.Any<DbModelField>() || model.KeyFields.Length == model.Fields.Length)
            {
                return;
            }

            stringGenerator.AppendLine("return \"UPDATE " + model.SchemaName + model.Name + "\\r\\n\" +");
            stringGenerator.PushIndent();

            var auditNames = parametersService.GetParameter(ParmType.NoUpdateAuditFields);
            var fields = model.Fields.Except(model.KeyFields).Where(x => !auditNames.Contains(x.FieldName)).ToArray();
            for (int i = 0; i < fields.Length; i++)
            {
                var linestart = i == 0 ? "SET " : "    ";
                var inlineend = i == fields.Length - 1 ? string.Empty : ",";
                stringGenerator.AppendLine("\"" + linestart + fields[i].FieldName + " = s." + fields[i].FieldName + inlineend + "\\r\\n\" +");
            }

            stringGenerator.AppendLine("\"FROM " + model.SchemaName + model.Name + " tj \\r\\n\" +");
            stringGenerator.AppendLine("\"INNER JOIN \" + table + \" s\\r\\n\" +");
            fields = model.KeyFields;
            for (int i = 0; i < fields.Length; i++)
            {
                var linestart = i == 0 ? "ON  " : "  AND ";
                var lineend = i == fields.Length - 1 ? ";" : " +";
                var inlineend = i == fields.Length - 1 ? string.Empty : ",";
                stringGenerator.AppendLine("\"" + linestart + "tj." + fields[i].FieldName + " = s." + fields[i].FieldName + inlineend + "\\r\\n\"" + lineend);
            }

            stringGenerator.PopIndent();
        }
    }
}
