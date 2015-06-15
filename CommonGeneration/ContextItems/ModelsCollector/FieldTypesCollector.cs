namespace ContextItems.CommonGeneration.ContextItems.ModelsCollector
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class FieldTypesCollector
    {
        public void UpdateFieldTypes(List<DbModel> models, IEnumerable<XElement> elements)
        {
            var dict = models.ToDictionary(x => x.Name, x => x);
            foreach (var element in elements)
            {
                DbModel model;
                if (!dict.TryGetValue(element.Attribute("Name").Value, out model))
                {
                    continue;
                }

                foreach (var source in element.Elements())
                {
                    if (source.Name.LocalName != "Property")
                    {
                        continue;
                    }

                    var field = model.Fields.FirstOrDefault(x => x.FieldName == source.Attribute("Name").Value)
                                ?? model.Fields.FirstOrDefault(x => x.FieldName.Substring(0, x.FieldName.Length - 1) == source.Attribute("Name").Value);
                    if (field == null)
                    {
                        continue;
                    }

                    field.StorageType = source.Attribute("Type").Value;
                    var nullableAttr = source.Attribute("Nullable");
                    field.Nullable = nullableAttr == null || nullableAttr.Value.ToLower() != "false";
                    var maxlenAttr = source.Attribute("MaxLength");
                    if (maxlenAttr != null)
                    {
                        field.Length = int.Parse(maxlenAttr.Value);
                    }
                }
            }
        }
    }
}
