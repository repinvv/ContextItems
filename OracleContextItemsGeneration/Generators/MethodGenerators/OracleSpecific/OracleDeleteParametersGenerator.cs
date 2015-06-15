namespace ContextItems.OracleContextItemsGeneration.Generators.MethodGenerators.OracleSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class OracleDeleteParametersGenerator : IMethodGenerator
    {
        private readonly ParametersGenerator parametersGenerator;

        public OracleDeleteParametersGenerator(ParametersGenerator parametersGenerator)
        {
            this.parametersGenerator = parametersGenerator;
        }

        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("void IOracleEntity.AddOracleDeleteParameters(IEnumerable<IOracleEntity> entities, OracleCommand command)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        { 
            var fields = model.KeyFields.Any<DbModelField>() ? model.KeyFields : model.Fields;
            parametersGenerator.GenerateParameters(model, fields, stringGenerator);
        }
    }
}
