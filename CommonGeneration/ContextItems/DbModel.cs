namespace ContextItems.CommonGeneration.ContextItems
{
    public class DbModel
    {
        public string Name { get; set; }

        public string SchemaName { get; set; }

        public DbModelField[] Fields { get; set; }

        public DbModelField[] KeyFields { get; set; }
    }
}
