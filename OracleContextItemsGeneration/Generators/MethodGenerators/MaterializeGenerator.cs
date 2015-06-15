namespace ContextItems.OracleContextItemsGeneration.Generators.MethodGenerators
{
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class MaterializeGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("void IOracleEntity.Materialize(IDataRecord input, IFieldsCaster caster)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            for (int index = 0; index < model.Fields.Length; index++)
            {
                var field = model.Fields[index];
                if (field.Type == typeof(string))
                {
                    stringGenerator.AppendLine(field.FieldName + " = input.IsDBNull(" + index + ") ? null : input.GetString(" + index + ");");
                }
                else if (CommonGen.IsNullable(field.Type))
                {
                    stringGenerator.AppendLine(field.FieldName + " = caster.GetNullableRecord<" + CommonGen.GetTypeName(CommonGen.GenArgument(field.Type)) +
                                               ">(" + index + ", input);");
                }
                else
                {
                    stringGenerator.AppendLine(field.FieldName + " = caster.GetRecord<" + CommonGen.GetTypeName(field.Type) +
                                               ">(" + index + ", input);");
                }
            }
        }
    }
}
