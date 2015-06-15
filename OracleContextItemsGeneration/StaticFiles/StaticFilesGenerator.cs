namespace ContextItems.OracleContextItemsGeneration.StaticFiles
{
    using System.Collections.Generic;
    using System.Linq;
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.StaticFiles;

    internal class StaticFilesGenerator
    {
        private readonly IFileGenerator[] generators;

        public StaticFilesGenerator(ConnectionHandlerGenerator connectionHandlerGenerator,
            OracleContextItemsGenerator oracleContextItemsGenerator,
            FieldsCasterInterfaceGenerator fieldsCasterInterfaceGenerator,
            OracleEntityInterfaceGenerator oracleEntityInterfaceGenerator,
            MaterializeDictionariesGenerator materializeDictionariesGenerator,
            MaterializerGenerator materializerGenerator,
            AdoCommandsGenerator adoCommandsGenerator,
            AdoCommandsInterfaceGenerator adoCommandsInterfaceGenerator)
        {
            generators = new IFileGenerator[]
                         {
                             connectionHandlerGenerator,
                             oracleContextItemsGenerator,
                             fieldsCasterInterfaceGenerator,
                             oracleEntityInterfaceGenerator,
                             materializeDictionariesGenerator,
                             materializerGenerator,
                             adoCommandsGenerator,
                             adoCommandsInterfaceGenerator
                         };
        }

        public IEnumerable<GeneratedFile> GenerateFiles(string outputNamespace, List<DbModel> models)
        {
            return generators.Select<IFileGenerator, GeneratedFile>(x => new GeneratedFile { Name = x.GetName(), Content = x.FileContents(outputNamespace, models) });
        }
    }
}
