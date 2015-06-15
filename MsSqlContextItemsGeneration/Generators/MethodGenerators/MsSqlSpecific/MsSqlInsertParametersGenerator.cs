namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class MsSqlInsertParametersGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("IEnumerable<SqlParameter> IMsSqlEntity.GetMsSqlInsertParameters(int index)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            int i = 1;
            foreach (var modelField in model.Fields.Where<DbModelField>(x => !x.IsIdentity))
            {
                var nullValue = modelField.Type.IsNullable() || modelField.Type == typeof(string)
                    ? " ?? (object)DBNull.Value"
                    : string.Empty;

                stringGenerator.AppendLine("yield return new SqlParameter(\"parm" + i++ + "i\" + index, " + modelField.FieldName + nullValue + ");");
            }
        }
    }
}
