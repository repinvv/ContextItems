namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.Parameters;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class MsSqlUpdateParametersGenerator : IMethodGenerator
    {
        private readonly IParametersService parametersService;

        public MsSqlUpdateParametersGenerator(IParametersService parametersService)
        {
            this.parametersService = parametersService;
        }

        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("IEnumerable<SqlParameter> IMsSqlEntity.GetUpdateParameters(int index)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        { 
            if (!model.KeyFields.Any<DbModelField>() || model.KeyFields.Length == model.Fields.Length)
            {
                return;
            }

            int i = 1;
            var auditNames = parametersService.GetParameter(ParmType.NoUpdateAuditFields);
            foreach (var modelField in model.Fields.Except(model.KeyFields).Where(x => !auditNames.Contains(x.FieldName)).Concat(model.KeyFields))
            {
                var nullValue = modelField.Type.IsNullable() || modelField.Type == typeof(string)
                    ? " ?? (object)DBNull.Value"
                    : string.Empty;

                stringGenerator.AppendLine("yield return new SqlParameter(\"parm" + i++ + "i\" + index, " + modelField.FieldName + nullValue + ");");
            }
        }
    }
}
