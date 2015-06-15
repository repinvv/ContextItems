namespace ContextItems.OracleContextItemsGeneration.Generators
{
    using System.Collections.Generic;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.OracleContextItemsGeneration.Generators.MethodGenerators;

    internal class CommonMethodsCollection
    {
        private readonly IMethodGenerator[] methodGenerators;

        public CommonMethodsCollection(EqualsGenerator equalsGenerator,
            TypedEqualsGenerator typedEqualsGenerator,
            GetHashCodeGenerator getHashCodeGenerator,
            MaterializeGenerator materializeGenerator,
            GetFieldTypesGenerator getFieldTypesGenerator)
        {
            methodGenerators = new IMethodGenerator[]
                               {
                                   equalsGenerator,
                                   typedEqualsGenerator,
                                   getHashCodeGenerator,
                                   materializeGenerator,
                                   getFieldTypesGenerator
                               };
        }

        public IEnumerable<IMethodGenerator> GetMethodsCollection()
        {
            return methodGenerators;
        }
    }
}
