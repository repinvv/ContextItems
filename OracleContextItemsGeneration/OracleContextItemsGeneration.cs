namespace ContextItems.OracleContextItemsGeneration
{
    using System.Collections.Generic;
    using System.Linq;
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems.ModelsCollector;
    using ContextItems.OracleContextItemsGeneration.Generators;

    public class OracleContextItemsGeneration
    {
        public IEnumerable<GeneratedFile> Generate(string edmxSchema, string outNamespace)
        {
            var container = new Container(this);
            var models = container.Get<ModelsCollector>().GetModels(edmxSchema).ToList();
            return container.Get<FilesGenerator>().GenerateFiles(models, outNamespace, GenerationConstants.OracleProvider);
        }
    }
}
