namespace ContextItems.CommonGeneration.ContextItems.Generators
{
    using System.Linq;
    using global::ContextItems.CommonGeneration.StringGenerator;

    public class TypedEqualsGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("public bool Equals(" + model.Name + " other)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        { 
            stringGenerator.AppendLine("if (other == null)");
            stringGenerator.Braces(() => stringGenerator.AppendLine("return false;"));
            stringGenerator.AppendLine();

            if (!model.KeyFields.Any())
            {
                stringGenerator.AppendLine("return false;");
                return;
            }

            for (int i = 0; i < model.KeyFields.Length; i++)
            {
                string linestart = "equal = equal && ";
                if (i == 0)
                {
                    linestart = "bool equal = ";
                }

                if (i == model.KeyFields.Length - 1)
                {
                    linestart = "return equal && ";
                }

                if (model.KeyFields.Length == 1)
                {
                    linestart = "return ";
                }

                stringGenerator.AppendLine(linestart + model.KeyFields[i].FieldName + " == other." + model.KeyFields[i].FieldName + ";");
            }
        }
    }
}
