namespace ContextItems.CommonGeneration.ContextItems.Generators
{
    using global::ContextItems.CommonGeneration.StringGenerator;

    public class EqualsGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        { 
            stringGenerator.AppendLine("public override bool Equals(object obj)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("return Equals(obj as " + model.Name + ");");
        }
    }
}
