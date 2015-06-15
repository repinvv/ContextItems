namespace ContextItems.OracleContextItemsGeneration.Generators.MethodGenerators.OracleSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class AddUpdateParametersGenerator : IMethodGenerator
    {
        private readonly ParametersGenerator parametersGenerator;

        public AddUpdateParametersGenerator(ParametersGenerator parametersGenerator)
        {
            this.parametersGenerator = parametersGenerator;
        }

        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("void IOracleEntity.AddOracleUpdateParameters(IEnumerable<IOracleEntity> entities, OracleCommand command)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            var fields = model.Fields.Except<DbModelField>(model.KeyFields).Concat(model.KeyFields).ToArray();
            parametersGenerator.GenerateParameters(model, fields, stringGenerator);
        }
    }
}
