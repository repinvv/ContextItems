namespace ContextItems.OracleContextItemsGeneration.Generators.MethodGenerators.OracleSpecific
{
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class ApplyOutParameterGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("void IOracleEntity.ApplyOutParameter<T>(OracleParameter outParameter, List<T> list)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            if (model.KeyFields.Length != 1 || model.KeyFields[0].IsIdentity == false)
            {
                return;
            }

            stringGenerator.AppendLine("dynamic result = outParameter.Value;");
            stringGenerator.AppendLine("for (int i = 0; i < result.Length && i < list.Count; i++)");
            stringGenerator.Braces(() => GenerateAssignment(model, stringGenerator));
        }

        private void GenerateAssignment(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("(list[i] as " + model.Name + ")." + model.KeyFields[0].FieldName + " = (" + model.KeyFields[0].Type.GetTypeName() + ")result[i];");
        }
    }
}
