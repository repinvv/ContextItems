namespace ContextItems.MsSqlContextItemsGeneration.StaticFiles
{
    using System.Collections.Generic;
    using System.Linq;
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.StaticFiles;

    internal class StaticFilesGenerator
    {
        private readonly IFileGenerator[] generators;

        public StaticFilesGenerator(BulkInsertDataReaderGenerator bulkInsertDataReaderGenerator,
            BulkInsertKeysDataReaderGenerator bulkInsertKeysDataReaderGenerator,
            ConnectionHandlerGenerator connectionHandlerGenerator,
            MaterialGenerator materialGenerator,
            MsSqlEntityGenerator mssqlEntityGenerator,
            ContextItemsGenerator contextItemsGenerator,
            AdoCommandsGenerator adoCommandsGenerator,
            AdoCommandsInterfaceGenerator adoCommandsInterfaceGenerator)
        {
            generators = new IFileGenerator[]
                         {
                             bulkInsertDataReaderGenerator,
                             bulkInsertKeysDataReaderGenerator,
                             connectionHandlerGenerator,
                             materialGenerator,
                             mssqlEntityGenerator,
                             contextItemsGenerator,
                             adoCommandsGenerator,
                             adoCommandsInterfaceGenerator
                         };
        }

        public IEnumerable<GeneratedFile> GenerateStaticFiles(string outputNamespace, IEnumerable<DbModel> models)
        {
            return generators.Select<IFileGenerator, GeneratedFile>(x => new GeneratedFile { Name = x.GetName(), Content = x.FileContents(outputNamespace, models) });
        }
    }
}
