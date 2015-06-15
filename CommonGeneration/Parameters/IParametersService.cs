namespace ContextItems.CommonGeneration.Parameters
{
    public interface IParametersService
    {
        void SetParameters(GenParameter[] parameters);

        string[] GetParameter(ParmType type);
    }
}