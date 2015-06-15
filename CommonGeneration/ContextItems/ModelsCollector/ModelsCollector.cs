namespace ContextItems.CommonGeneration.ContextItems.ModelsCollector
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class ModelsCollector
    {
        private readonly ModelCollector modelCollector;
        private readonly FieldTypesCollector fieldTypesCollector;
        private readonly SchemaNamesCollector schemaNamesCollector;

        public ModelsCollector(ModelCollector modelCollector, FieldTypesCollector fieldTypesCollector, SchemaNamesCollector schemaNamesCollector)
        {
            this.modelCollector = modelCollector;
            this.fieldTypesCollector = fieldTypesCollector;
            this.schemaNamesCollector = schemaNamesCollector;
        }

        public IEnumerable<DbModel> GetModels(string edmxSchema)
        {
            const string SchemaName = "http://schemas.microsoft.com/ado/2009/11/edmx";
            var xdoc = XDocument.Load(edmxSchema);
            var namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("edmx", SchemaName);
            var elements = xdoc.XPathSelectElement("/edmx:Edmx/edmx:Runtime/edmx:ConceptualModels", namespaceManager)
                               .Elements()
                               .First()
                               .Elements()
                               .Where(x => x.Name.LocalName == "EntityType");
            var models = elements.Select(x => modelCollector.GetModel(x)).ToList();
            elements = xdoc.XPathSelectElement("/edmx:Edmx/edmx:Runtime/edmx:StorageModels", namespaceManager)
                               .Elements()
                               .First()
                               .Elements()
                               .Where(x => x.Name.LocalName == "EntityType");
            fieldTypesCollector.UpdateFieldTypes(models, elements);
            elements = xdoc.XPathSelectElement("/edmx:Edmx/edmx:Runtime/edmx:StorageModels", namespaceManager)
                           .Elements()
                           .First()
                           .Elements();
            var a = elements
                           .First(x => x.Name.LocalName == "EntityContainer");
            elements = a.Elements()
                        .Where(x => x.Name.LocalName == "EntitySet");
            schemaNamesCollector.UpdateSchemaName(models, elements);
            return models;
        }
    }
}