namespace ContextItems.CommonGeneration.ContextItems.Generators
{
    using global::ContextItems.CommonGeneration.StringGenerator;

    public interface IMethodGenerator
    {
        void GenerateSignature(DbModel model, IStringGenerator stringGenerator);

        void GenerateContent(DbModel model, IStringGenerator stringGenerator);
    }
}
