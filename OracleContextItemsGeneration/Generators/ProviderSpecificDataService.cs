namespace ContextItems.OracleContextItemsGeneration.Generators
{
    using System.Collections.Generic;
    using System.Linq;
    using ContextItems.CommonGeneration;
    using ContextItems.CommonGeneration.ContextItems.Generators;

    internal class ProviderSpecificDataService
    {
        private readonly CommonMethodsCollection commonMethodsCollection;
        private readonly OracleMethodsCollection oracleMethodsCollection;

        public ProviderSpecificDataService(CommonMethodsCollection commonMethodsCollection,
            OracleMethodsCollection oracleMethodsCollection)
        {
            this.commonMethodsCollection = commonMethodsCollection;
            this.oracleMethodsCollection = oracleMethodsCollection;
        }

        public IEnumerable<IMethodGenerator> GetMethodGenerators(string provider)
        {
            return commonMethodsCollection.GetMethodsCollection().Concat(oracleMethodsCollection.GetMethodsCollection());
        }

        public IEnumerable<string> GetUsings(string provider)
        {
            switch (provider)
            {
                case GenerationConstants.OracleProvider:
                    return GenerationConstants.OraclePartialsUsings.Concat(GenerationConstants.PartialsUsings);
                case GenerationConstants.MsSqlProvider:
                    return GenerationConstants.MsSqlPartialsUsing.Concat(GenerationConstants.PartialsUsings);
                default:
                    return null;
            }
        }
    }
}
