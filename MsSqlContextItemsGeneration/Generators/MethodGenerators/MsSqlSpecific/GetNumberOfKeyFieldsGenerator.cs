namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class GetNumberOfKeyFieldsGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("int IMsSqlEntity.GetNumberOfKeyFields()");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            var fields = model.KeyFields.Any<DbModelField>() ? model.KeyFields : model.Fields;
            stringGenerator.AppendLine(" return " + fields.Length + ";");
        }
    }
}
