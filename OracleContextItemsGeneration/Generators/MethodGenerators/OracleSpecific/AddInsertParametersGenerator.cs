namespace ContextItems.OracleContextItemsGeneration.Generators.MethodGenerators.OracleSpecific
{
    using System.Linq;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class AddInsertParametersGenerator : IMethodGenerator
    {
        private readonly ParametersGenerator parametersGenerator;

        public AddInsertParametersGenerator(ParametersGenerator parametersGenerator)
        {
            this.parametersGenerator = parametersGenerator;
        }

        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("OracleParameter IOracleEntity.AddOracleInsertParameters(IEnumerable<IOracleEntity> entities, OracleCommand command)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        { 
            if (model.KeyFields.Length != 1 || model.KeyFields[0].IsIdentity == false)
            {
                stringGenerator.AppendLine("((IOracleEntity)this).AddOracleUpdateParameters(entities, command);");
                stringGenerator.AppendLine("return null;");
                return;
            }

            var fields = model.Fields.Except<DbModelField>(model.KeyFields).ToArray();
            parametersGenerator.GenerateParameters(model, fields, stringGenerator);
            var type = parametersGenerator.GetOracleDbType(model.KeyFields.First());
            stringGenerator.AppendLine("return command.Parameters.Add(\"id\", OracleDbType." + type + ", ParameterDirection.Output);");
        }
    }
}
