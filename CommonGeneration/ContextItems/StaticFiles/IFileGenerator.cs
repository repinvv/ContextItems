namespace ContextItems.CommonGeneration.ContextItems.StaticFiles
{
    using System.Collections.Generic;

    public interface IFileGenerator
    {
        string GetName();

        string FileContents(string outNamespace, IEnumerable<DbModel> models);
    }
}
