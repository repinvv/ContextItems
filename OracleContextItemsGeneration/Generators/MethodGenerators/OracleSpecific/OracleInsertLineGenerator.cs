namespace ContextItems.OracleContextItemsGeneration.Generators.MethodGenerators.OracleSpecific
{
    using System;
    using System.Linq;
    using ContextItems.CommonGeneration.ContextItems;
    using ContextItems.CommonGeneration.ContextItems.Generators;
    using ContextItems.CommonGeneration.StringGenerator;

    internal class OracleInsertLineGenerator : IMethodGenerator
    {
        public void GenerateSignature(DbModel model, IStringGenerator stringGenerator)
        {
            stringGenerator.AppendLine("string IOracleEntity.GetOracleInsertRequest(string schemaName)");
        }

        public void GenerateContent(DbModel model, IStringGenerator stringGenerator)
        {
            DbModelField[] fields;
            string returning;
            if (model.KeyFields.Length == 1 && model.KeyFields[0].IsIdentity)
            {
                fields = model.Fields.Except<DbModelField>(model.KeyFields).ToArray();
                returning = "RETURNING " + model.KeyFields[0].FieldName + " INTO :id";
            }
            else
            {
                fields = model.Fields.Except(model.KeyFields).Concat(model.KeyFields).ToArray();
                returning = string.Empty;
            }

            if (fields.Length > 10)
            {
                GenerateLongVersion(model, stringGenerator, fields, returning);
                return;
            }

            if (fields.Length > 5)
            {
                GenerateMediumVersion(model, stringGenerator, fields, returning);
                return;
            }
            
            int i = 1;
            var fieldNames = string.Join(", ", fields.Select(x => x.FieldName));
            var parameters = string.Join(", ", fields.Select(x => ":parm" + i++));

            stringGenerator.AppendLine("return \"INSERT INTO \" + schemaName + \"" + model.Name + " (" + fieldNames + ") VALUES (" + parameters + ") " + returning + "\";");
        }

        private void GenerateMediumVersion(DbModel model, IStringGenerator stringGenerator, DbModelField[] fields, string returning)
        {
            int i = 1;
            var fieldNames = string.Join(", ", fields.Select(x => x.FieldName));
            var parameters = string.Join(", ", fields.Select(x => ":parm" + i++));

            stringGenerator.AppendLine("return \"INSERT INTO \" + schemaName + \"" + model.Name + "\\r\\n\"");
            stringGenerator.PushIndent();
            stringGenerator.AppendLine("+ \"    (" + fieldNames + ") VALUES\\r\\n\"");
            stringGenerator.AppendLine("+ \"    (" + parameters + ") " + returning + " \";");
            stringGenerator.PopIndent();
        }

        private void GenerateLongVersion(DbModel model, IStringGenerator stringGenerator, DbModelField[] modelFields, string returning)
        {
            stringGenerator.AppendLine("return \"INSERT INTO \" + schemaName + \"" + model.Name + "\\r\\n\"");
            var parts = (modelFields.Length / 10) + (modelFields.Length % 10 == 0 ? 0 : 1);
            var increment = (modelFields.Length / parts) + (modelFields.Length % parts == 0 ? 0 : 1);
            for (int n = 0; n < modelFields.Length; n += increment)
            {
                var amount = Math.Min(modelFields.Length - n, increment);
                var fields = modelFields.Skip(n).Take(amount).ToArray();
                var startline = n == 0 ? "    (" : "     ";
                var endline = n + amount == modelFields.Length ? ") VALUES" : ",";
                var fieldNames = string.Join(", ", fields.Select(x => x.FieldName));
                stringGenerator.AppendLine("+ \"" + startline + fieldNames + endline + "\\r\\n\"");
            }

            for (int n = 0; n < modelFields.Length; n += increment)
            {
                var amount = Math.Min(modelFields.Length - n, increment);
                var fields = modelFields.Skip(n).Take(amount).ToArray();
                var startline = n == 0 ? "    (" : "     ";
                var endline = n + amount == modelFields.Length ? ") " + returning + "\";" : ",\\r\\n\"";
                int i = n + 1;
                var line = string.Join(", ", fields.Select(x => ":parm" + i++));
                stringGenerator.AppendLine("+ \"" + startline + line + endline);
            }
        }
    }
}
