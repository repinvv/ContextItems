namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class ValueGenerator
    {
        public void GenerateGetValue(DbModelField[] fields, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("switch (i)");
            stringGenerator.Braces(() => GenerateSwitchContents(fields, stringGenerator));
            stringGenerator.AppendLine();
            stringGenerator.AppendLine("return null;");
        }

        private void GenerateSwitchContents(DbModelField[] fields, IStringGenerator stringGenerator)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                stringGenerator.AppendLine("case " + i + ":");
                stringGenerator.PushIndent();
                stringGenerator.AppendLine("return " + fields[i].FieldName + ";");
                stringGenerator.PopIndent();
            }
        }
    }
}
