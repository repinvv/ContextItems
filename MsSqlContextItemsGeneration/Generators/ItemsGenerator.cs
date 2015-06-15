namespace ContextItems.MsSqlContextItemsGeneration.Generators
{
    using System.Linq;
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class ItemsGenerator
    {
        private readonly IStringGenerator stringGenerator;
        private readonly UsingsGenerator usingsGenerator;
        private readonly CommonMethodsCollection commonMethodsCollection;
        private readonly MsSqlMethodsCollection mssqlMethodsCollection;

        public ItemsGenerator(IStringGenerator stringGenerator,
                              UsingsGenerator usingsGenerator,
                              CommonMethodsCollection commonMethodsCollection,
                              MsSqlMethodsCollection mssqlMethodsCollection)
        {
            this.stringGenerator = stringGenerator;
            this.usingsGenerator = usingsGenerator;
            this.commonMethodsCollection = commonMethodsCollection;
            this.mssqlMethodsCollection = mssqlMethodsCollection;
        }

        public GeneratedFile GeneratePartial(DbModel model, string outNamespace, string provider)
        {
            GenerateContents(model, outNamespace, provider);

            return new GeneratedFile
                   {
                       Name = model.Name + "_partial_" + provider,
                       Content = stringGenerator.ToString()
                   };
        }

        private void GenerateContents(DbModel model, string outNamespace, string provider)
        {
            stringGenerator.AppendLine("namespace " + outNamespace);
            stringGenerator.Braces(() => GenerateClass(model, provider));
        }

        private void GenerateClass(DbModel model, string provider)
        {
            usingsGenerator.GenerateUsings(stringGenerator, GenerationConstants.MsSqlPartialsUsing.Concat(GenerationConstants.PartialsUsings));
            stringGenerator.AppendLine();
            stringGenerator.AppendLine("public partial class " + model.Name + " : " + GenerationConstants.MsSqlEntity);
            stringGenerator.Braces(() => GenerateClassContents(model, provider));
        }

        private void GenerateClassContents(DbModel model, string provider)
        {
            stringGenerator.AppendLine("public static string SequenceName = \"" + model.Name + "_seq\";");
            var methodGenerators = commonMethodsCollection.GetMethodsCollection().Concat(mssqlMethodsCollection.GetMethodsCollection()).ToArray();
            for (int index = 0; index < methodGenerators.Length; index++)
            {
                if (index > 0)
                {
                    stringGenerator.AppendLine();
                }

                var generator = methodGenerators[index];
                generator.GenerateSignature(model, stringGenerator);
                stringGenerator.Braces(() => generator.GenerateContent(model, stringGenerator));
            }
        }
    }
}
