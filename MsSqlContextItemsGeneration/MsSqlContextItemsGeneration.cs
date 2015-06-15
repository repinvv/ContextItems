namespace ContextItems.MsSqlContextItemsGeneration
{
    using System.Collections.Generic;
    using System.Linq;
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems.ModelsCollector;
    using ContextItems.CommonGeneration.Parameters;
    using ContextItems.MsSqlContextItemsGeneration.Generators;

    public class MsSqlContextItemsGeneration
    {
        public IEnumerable<GeneratedFile> Generate(string edmxSchema, string outNamespace, params GenParameter[] parameters)
        {
            var container = new Container(this);
            container.Get<IParametersService>().SetParameters(parameters);
            var models = container.Get<ModelsCollector>().GetModels(edmxSchema).ToList();
            var output = container.Get<FilesGenerator>().GenerateFiles(models, outNamespace, GenerationConstants.MsSqlProvider).ToList();
            return output;
        }
    }
}
