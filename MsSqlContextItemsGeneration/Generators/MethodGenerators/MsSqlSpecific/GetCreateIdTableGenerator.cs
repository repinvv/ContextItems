namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class GetCreateIdTableGenerator : IMethodGenerator
    {
        private readonly CreateTableGenerator createTableGenerator;

        public GetCreateIdTableGenerator(CreateTableGenerator createTableGenerator)
        {
            this.createTableGenerator = createTableGenerator;
        }

        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("string IMsSqlEntity.GetCreateIdTableRequest(string table)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            var fields = model.KeyFields.Any<DbModelField>() ? model.KeyFields : model.Fields;
            createTableGenerator.GenerateCreateTable(fields, stringGenerator);
        }
    }
}
