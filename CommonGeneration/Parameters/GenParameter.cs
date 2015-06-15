namespace ContextItems.CommonGeneration.Parameters
{
    /// <summary>
    /// Generation Parameter
    /// </summary>
    public class GenParameter
    {
        public ParmType ParmType { get; set; }

        public string[] Values { get; set; }

        public GenParameter(ParmType parmType, params string[] values)
        {
            ParmType = parmType;
            Values = values;
        }
    }
}
