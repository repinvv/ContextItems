namespace ContextItems.MsSqlContextItemsGeneration.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.MsSqlContextItemsGeneration.StaticFiles;

    internal class FilesGenerator
    {
        private readonly ItemsGenerator partialGenerator;
        private readonly StaticFilesGenerator staticFilesGenerator;

        public FilesGenerator(ItemsGenerator partialGenerator, StaticFilesGenerator staticFilesGenerator)
        {
            this.partialGenerator = partialGenerator;
            this.staticFilesGenerator = staticFilesGenerator;
        }

        public IEnumerable<GeneratedFile> GenerateFiles(List<DbModel> models, string outNamespace, string provider)
        {
            var result = models.Select(x => partialGenerator.GeneratePartial(x, outNamespace, provider)).ToList();
            result.AddRange(staticFilesGenerator.GenerateStaticFiles(outNamespace, models));
            result.ForEach(x => x.Content = GenerationConstants.GenerationMark + Environment.NewLine + x.Content);
            return result;
        }
    }
}
