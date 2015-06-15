namespace ContextItems.OracleContextItemsGeneration.Generators
{
    using System.Collections.Generic;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.OracleContextItemsGeneration.Generators.MethodGenerators.OracleSpecific;

    internal class OracleMethodsCollection
    {
        private readonly IMethodGenerator[] methodGenerators;

        public OracleMethodsCollection(OracleInsertLineGenerator oracleInsertLineGenerator,
            OracleUpdateLineGenerator oracleUpdateLineGenerator,
            AddUpdateParametersGenerator addUpdateParametersGenerator,
            AddInsertParametersGenerator addInsertParametersGenerator,
            ApplyOutParameterGenerator applyOutParameterGenerator,
            OracleDeleteLineGenerator oracleDeleteLineGenerator,
            OracleDeleteParametersGenerator oracleDeleteParametersGenerator)
        {
            methodGenerators = new IMethodGenerator[]
                               {
                                   oracleInsertLineGenerator,
                                   oracleUpdateLineGenerator,
                                   oracleDeleteLineGenerator,
                                   addInsertParametersGenerator,
                                   addUpdateParametersGenerator,
                                   oracleDeleteParametersGenerator,
                                   applyOutParameterGenerator
                               };
        }

        public IEnumerable<IMethodGenerator> GetMethodsCollection()
        {
            return methodGenerators;
        }
    }
}
