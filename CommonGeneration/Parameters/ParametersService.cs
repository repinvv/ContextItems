namespace ContextItems.CommonGeneration.Parameters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class ParametersService : IParametersService
    {
        private Dictionary<ParmType, string[]> paramDict = new Dictionary<ParmType, string[]>();

        public void SetParameters(GenParameter[] parameters)
        {
            paramDict = parameters.ToDictionary(x => x.ParmType, x => x.Values);
        }

        public string[] GetParameter(ParmType type)
        {
            string[] value;
            if (paramDict.TryGetValue(type, out value))
            {
                return value;
            }

            return new string[0];
        }
    }
}
