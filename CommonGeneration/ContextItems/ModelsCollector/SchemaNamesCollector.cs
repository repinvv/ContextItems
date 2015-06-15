namespace ContextItems.CommonGeneration.ContextItems.ModelsCollector
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class SchemaNamesCollector
    {
        public void UpdateSchemaName(List<DbModel> models, IEnumerable<XElement> elements)
        {
            var dict = models.ToDictionary(x => x.Name, x => x);
            foreach (var element in elements)
            {
                DbModel model;
                if (!dict.TryGetValue(element.Attribute("Name").Value, out model))
                {
                    continue;
                }

                var schemaName = element.Attribute("Schema").Value;
                if (!string.IsNullOrWhiteSpace(schemaName))
                {
                    model.SchemaName = schemaName + ".";
                }
            }
        }
    }
}
