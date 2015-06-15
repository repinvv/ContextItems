namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class GetDeleteParametersGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("IEnumerable<SqlParameter> IMsSqlEntity.GetDeleteParameters(int index)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            int i = 1;
            foreach (var modelField in model.KeyFields)
            {
                stringGenerator.AppendLine("yield return new SqlParameter(\"parm" + i++ + "i\" + index, " + modelField.FieldName + ");");
            }
        }
    }
}
