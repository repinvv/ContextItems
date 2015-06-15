namespace ContextItems.CommonGeneration.ContextItems
{
    using System;

    public class DbModelField
    {
        public string FieldName { get; set; }

        public Type Type { get; set; }

        public string TypeName { get; set; }

        public bool IsNullable { get; set; }

        public bool IsIdentity { get; set; }

        public string StorageType { get; set; }

        public int Length { get; set; }

        public bool Nullable { get; set; }
    }
}
