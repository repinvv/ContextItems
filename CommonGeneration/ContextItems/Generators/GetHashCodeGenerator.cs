namespace ContextItems.CommonGeneration.ContextItems.Generators
{
    using System.Linq;
    using global::ContextItems.CommonGeneration.StringGenerator;

    public class GetHashCodeGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        { 
            stringGenerator.AppendLine("public override int GetHashCode()");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("unchecked");
            stringGenerator.Braces(() => GenerateHashCreation(model, stringGenerator));
        }

        private void GenerateHashCreation(DbModel model, IStringGenerator stringGenerator)
        {
            if (!model.KeyFields.Any())
            {
                stringGenerator.AppendLine("return 0;");
                return;
            }

            for (int i = 0; i < model.KeyFields.Length; i++)
            {
                string linestart = "hash = (hash * 397) ^ ";
                if (i == 0)
                {
                    linestart = "int hash = ";
                }

                if (i == model.KeyFields.Length - 1)
                {
                    linestart = "return (hash * 397) ^ ";
                }

                if (model.KeyFields.Length == 1)
                {
                    linestart = "return ";
                }
                
                stringGenerator.AppendLine(linestart + model.KeyFields[i].FieldName + ".GetHashCode();");
            }
        }
    }
}
