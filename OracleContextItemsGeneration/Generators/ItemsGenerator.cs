namespace ContextItems.OracleContextItemsGeneration.Generators
{
    using System.Linq;
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class ItemsGenerator
    {
        private readonly IStringGenerator stringGenerator;
        private readonly UsingsGenerator constructorGenerator;
        private readonly CommonMethodsCollection commonMethodsCollection;
        private readonly OracleMethodsCollection oracleMethodsCollection;

        public ItemsGenerator(IStringGenerator stringGenerator,
                              UsingsGenerator constructorGenerator,
                              CommonMethodsCollection commonMethodsCollection,
                              OracleMethodsCollection oracleMethodsCollection)
        {
            this.stringGenerator = stringGenerator;
            this.constructorGenerator = constructorGenerator;
            this.commonMethodsCollection = commonMethodsCollection;
            this.oracleMethodsCollection = oracleMethodsCollection;
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
            constructorGenerator.GenerateUsings(stringGenerator, GenerationConstants.OraclePartialsUsings.Concat(GenerationConstants.PartialsUsings));
            stringGenerator.AppendLine();
            stringGenerator.AppendLine("public partial class " + model.Name + " : " + GenerationConstants.OracleEntity);
            stringGenerator.Braces(() => GenerateClassContents(model, provider));
        }

        private void GenerateClassContents(DbModel model, string provider)
        {
            var methodGenerators = commonMethodsCollection
                .GetMethodsCollection()
                .Concat(oracleMethodsCollection.GetMethodsCollection())
                .ToArray();
            for (int index = 0; index < methodGenerators.Length; index++)
            {
                if (index > 0)
                {
                    stringGenerator.AppendLine();
                }

                methodGenerators[index].GenerateSignature(model, stringGenerator);
                stringGenerator.Braces(() => methodGenerators[index].GenerateContent(model, stringGenerator));
            }
        }
    }
}
