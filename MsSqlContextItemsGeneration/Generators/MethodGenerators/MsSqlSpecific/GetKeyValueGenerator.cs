namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class GetKeyValueGenerator : IMethodGenerator
    {
        private readonly ValueGenerator valueGenerator;

        public GetKeyValueGenerator(ValueGenerator valueGenerator)
        {
            this.valueGenerator = valueGenerator;
        }

        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("object IMsSqlEntity.GetKeyValue(int i)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            if (!model.KeyFields.Any<DbModelField>())
            {
                stringGenerator.AppendLine("return GetValue(i);");
                return;
            }

            valueGenerator.GenerateGetValue(model.KeyFields, stringGenerator);
        }
    }
}
