namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class PopulateIdentityGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("void IMsSqlEntity.PopulateIdentity(IDataRecord input)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        { 
            var idField = model.Fields.FirstOrDefault<DbModelField>(x => x.IsIdentity);
            if (idField != null)
            {
                stringGenerator.AppendLine(idField.FieldName + " = (" + idField.Type.GetTypeName() + ")input[\"" + idField.FieldName + "\"];");
            }
        }
    }
}
