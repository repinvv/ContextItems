namespace ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific
{
    using System;
    using System.Linq;
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class SetSequenceGenerator : IMethodGenerator
    {
        private static Type[] seqTypes = new[] { typeof(short), typeof(int), typeof(long) };

        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("void IMsSqlEntity.SetSequence<T>(List<T> list, object seq)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            if (model.KeyFields.Length != 1 || !seqTypes.Contains(model.KeyFields[0].Type))
            {
                stringGenerator.AppendLine("throw new Exception(\"Sequenced insert is not applicable to this entity\");");
                return;
            }

            stringGenerator.AppendLine("var sequence = (" + CommonGen.GetTypeName(model.KeyFields[0].Type) + ")seq;");
            stringGenerator.AppendLine("list.ForEach(x => (x as " + model.Name + ")." + model.KeyFields[0].FieldName + " = sequence++);");
        }
    }
}
