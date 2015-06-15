namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class GetCreateTableGenerator : IMethodGenerator
    {
        private readonly CreateTableGenerator createTableGenerator;

        public GetCreateTableGenerator(CreateTableGenerator createTableGenerator)
        {
            this.createTableGenerator = createTableGenerator;
        }

        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("string IMsSqlEntity.GetCreateTempTableRequest(string table)"); 
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            createTableGenerator.GenerateCreateTable(model.Fields, stringGenerator);
        }
    }
}
