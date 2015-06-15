namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class NumberOfFieldsGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("int IMsSqlEntity.GetNumberOfFields()");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        { 
            stringGenerator.AppendLine("return " + model.Fields.Length + ";");
        }
    }
}
