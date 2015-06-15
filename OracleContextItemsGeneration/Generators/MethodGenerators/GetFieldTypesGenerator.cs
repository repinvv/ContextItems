namespace ContextItems.OracleContextItemsGeneration.Generators.MethodGenerators
{
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class GetFieldTypesGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("Type[] IOracleEntity.GetFieldTypes()");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("return new[]");
            stringGenerator.Braces(() => GenerateFieldTypes(model, stringGenerator), true);
        }

        private void GenerateFieldTypes(DbModel model, IStringGenerator stringGenerator)
        {
            for (int i = 0; i < model.Fields.Length; i++)
            {
                var field = model.Fields[i];
                if (CommonGen.IsNullable(field.Type))
                {
                    stringGenerator.AppendLine("typeof(" + CommonGen.GetTypeName(CommonGen.GenArgument(field.Type)) + "),");
                }
                else
                {
                    stringGenerator.AppendLine("typeof(" + CommonGen.GetTypeName(field.Type) + "),");
                }
            }
        }
    }
}
