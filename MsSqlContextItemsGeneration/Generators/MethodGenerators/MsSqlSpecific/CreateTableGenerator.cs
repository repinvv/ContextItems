namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class CreateTableGenerator
    {
        public void GenerateCreateTable(DbModelField[] fields, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("return \"CREATE TABLE \" + table + \"\\r\\n\" +");
            stringGenerator.PushIndent();
            stringGenerator.AppendLine("\"(\\r\\n\" +");
            foreach (var field in fields)
            {
                stringGenerator.AppendLine("\"" + field.FieldName + " " + field.StorageType + ",\\r\\n\" +");
            }

            stringGenerator.AppendLine("\")\";");
            stringGenerator.PopIndent();
        }
    }
}
