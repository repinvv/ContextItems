namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class MsSqlInsertGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("string IMsSqlEntity.GetMsSqlInsertRequest()");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            var fields = model.Fields.Where<DbModelField>(x => !x.IsIdentity).ToList();
            stringGenerator.AppendLine("return \"INSERT INTO " + model.SchemaName + model.Name + "\\r\\n\" +");
            var fieldsLine = string.Join(", ", fields.Select(x => x.FieldName));
            stringGenerator.PushIndent();
            stringGenerator.AppendLine("\"  (" + fieldsLine + ")\\r\\n\" +");
            var idField = model.Fields.FirstOrDefault(x => x.IsIdentity);
            if (idField != null)
            {
                stringGenerator.AppendLine("\"OUTPUT inserted." + idField.FieldName + "\\r\\n\" +");
            }

            stringGenerator.AppendLine("\"VALUES\\r\\n\";");
            stringGenerator.PopIndent();
        }
    }
}
