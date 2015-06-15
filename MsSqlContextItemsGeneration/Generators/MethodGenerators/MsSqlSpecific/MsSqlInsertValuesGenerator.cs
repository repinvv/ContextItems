namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class MsSqlInsertValuesGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("string IMsSqlEntity.GetMsSqlInsertValues(int i)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        { 
            var fields = model.Fields.Where<DbModelField>(x => !x.IsIdentity).ToList();
            int i = 1;
            var paramLine = string.Join(", ", fields.Select(x => "@parm" + i++ + "i\" + i + \""));
            stringGenerator.AppendLine("return \"(" + paramLine + ")\";");
        }
    }
}
