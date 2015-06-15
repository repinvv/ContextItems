namespace ContextItems.MsSqlContextItemsGeneration.Generators
{
    using System.Collections.Generic;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.MsSqlContextItemsGeneration.Generators.MethodGenerators.MsSqlSpecific;

    internal class MsSqlMethodsCollection
    {
        private readonly IMethodGenerator[] methodGenerators;

        public MsSqlMethodsCollection(NumberOfFieldsGenerator numberOfFieldsGenerator,
            HasIdentityGenerator hasIdentityGenerator,
            MsSqlInsertGenerator insertGenerator,
            MsSqlInsertValuesGenerator insertValuesGenerator,
            MsSqlInsertParametersGenerator insertParametersGenerator,
            PopulateIdentityGenerator populateIdentityGenerator,
            GetValueGenerator getValueGenerator,
            MsSqlUpdateGenerator updateGenerator,
            MsSqlUpdateParametersGenerator updateParametersGenerator,
            GetDeleteRequestGenerator getDeleteRequestGenerator,
            GetDeleteParametersGenerator getDeleteParametersGenerator,
            GetBulkUpdateRequestGenerator getBulkUpdateRequestGenerator,
            GetBulkDeleteRequestGenerator getBulkDeleteRequestGenerator,
            GetCreateTableGenerator getCreateTableGenerator,
            GetCreateIdTableGenerator getCreateIdTableGenerator,
            GetNumberOfKeyFieldsGenerator getNumberOfKeyFieldsGenerator,
            GetKeyValueGenerator getKeyValueGenerator,
            GetSequenceNameGenerator getSequenceNameGenerator,
            SetSequenceGenerator setSequenceGenerator,
            GetTableNameGenerator getTableNameGenerator)
        {
            methodGenerators = new IMethodGenerator[]
                               {
                                   hasIdentityGenerator,
                                   numberOfFieldsGenerator,
                                   getTableNameGenerator,
                                   insertGenerator,
                                   insertValuesGenerator,
                                   insertParametersGenerator,
                                   populateIdentityGenerator,
                                   getValueGenerator,
                                   getSequenceNameGenerator,
                                   setSequenceGenerator,
                                   updateGenerator,
                                   updateParametersGenerator,
                                   getBulkUpdateRequestGenerator,
                                   getCreateTableGenerator,
                                   getNumberOfKeyFieldsGenerator,
                                   getDeleteRequestGenerator,
                                   getDeleteParametersGenerator,
                                   getBulkDeleteRequestGenerator,
                                   getCreateIdTableGenerator,
                                   getKeyValueGenerator
                               };
        }

        public IEnumerable<IMethodGenerator> GetMethodsCollection()
        {
            return methodGenerators;
        }
    }
}
