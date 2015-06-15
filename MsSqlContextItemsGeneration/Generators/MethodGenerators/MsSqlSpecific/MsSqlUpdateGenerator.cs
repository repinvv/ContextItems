namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.Parameters;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class MsSqlUpdateGenerator : IMethodGenerator
    {
        private readonly IParametersService parametersService;

        public MsSqlUpdateGenerator(IParametersService parametersService)
        {
            this.parametersService = parametersService;
        }

        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("string IMsSqlEntity.GetUpdateRequest(int index)");
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
            int n = 1;
            for (int i = 0; i < fields.Length; i++)
            {
                var linestart = i == 0 ? "SET " : "    ";
                var inlineend = i == fields.Length - 1 ? string.Empty : ",";
                stringGenerator.AppendLine("\"" + linestart + fields[i].FieldName + " = @parm" + n++ + "i\" + index + \"" + inlineend + "\\r\\n\" +");
            }

            for (int i = 0; i < model.KeyFields.Length; i++)
            {
                var linestart = i == 0 ? "WHERE " : "  AND ";
                var lineend = i == model.KeyFields.Length - 1 ? ";" : " +";
                var inlineend = i == model.KeyFields.Length - 1 ? ";" : ",";
                stringGenerator.AppendLine("\"" + linestart + model.KeyFields[i].FieldName + " = @parm" + n++ + "i\" + index + \"" + inlineend + "\"" + lineend);
            }

            stringGenerator.PopIndent();
        }
    }
}
