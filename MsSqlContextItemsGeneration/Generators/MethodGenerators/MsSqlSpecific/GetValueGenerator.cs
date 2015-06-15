namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.StringGenerator;
    using ContextItems.CommonGeneration.ContextItems.Generators;

    internal class GetValueGenerator : IMethodGenerator
    {
        private readonly ValueGenerator valueGenerator;

        public GetValueGenerator(ValueGenerator valueGenerator)
        {
            this.valueGenerator = valueGenerator;
        }

        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("object IMsSqlEntity.GetValue(int i)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            valueGenerator.GenerateGetValue(model.Fields, stringGenerator);
        }
    }
}
