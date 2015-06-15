namespace ContextItems.MsSqlContextItemsGeneration.Generators
{
    using System.Collections.Generic;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators;

    internal class CommonMethodsCollection
    {
        private readonly IMethodGenerator[] methodGenerators;

        public CommonMethodsCollection(EqualsGenerator equalsGenerator,
            TypedEqualsGenerator typedEqualsGenerator,
            GetHashCodeGenerator getHashCodeGenerator,
            MaterializeGenerator materializeGenerator)
        {
            methodGenerators = new IMethodGenerator[]
                               {
                                   equalsGenerator,
                                   typedEqualsGenerator,
                                   getHashCodeGenerator,
                                   materializeGenerator,
                               };
        }

        public IEnumerable<IMethodGenerator> GetMethodsCollection()
        {
            return methodGenerators;
        }
    }
}
