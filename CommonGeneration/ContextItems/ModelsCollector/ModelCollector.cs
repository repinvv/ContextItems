namespace ContextItems.CommonGeneration.ContextItems.ModelsCollector
{
    using System.Linq;
    using System.Xml.Linq;
    using global::ContextItems.CommonGeneration.ContextItems.ModelFieldCollector;

    public class ModelCollector
    {
        private readonly ModelFieldCollector modelFieldsCollector;

        public ModelCollector(ModelFieldCollector modelFieldsCollector)
        {
            this.modelFieldsCollector = modelFieldsCollector;
        }

        public DbModel GetModel(XElement xelement)
        {
            var attributes = xelement.Attributes();
            var elements = xelement.Elements().ToArray();
            var fields = elements.Where(x => x.Name.LocalName == "Property")
                                 .Select(x => modelFieldsCollector.GetModelField(x)).ToArray();
            var keyFields = modelFieldsCollector.GetKeyFields(elements.First(x => x.Name.LocalName == "Key"), fields).ToArray();
            return new DbModel
                   {
                       Name = attributes.First(x => x.Name == "Name").Value,
                       Fields = fields,
                       KeyFields = keyFields
                   };
        }
    }
}