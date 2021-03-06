﻿namespace ContextItems.CommonGeneration
{
    public static class GenerationConstants
    {
        public const string OracleProvider = "oracle";

        public const string MsSqlProvider = "mssql";

        public const string GenerationMark = 
@"//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------";

        public const int IndentSize = 4;

        public const string Subquery = "subquery";

        public const string Split = "split";

        public const string Temp = "temp";

        public const string Repo = "repo";

        public const string OracleEntity = "IOracleEntity";

        public const string MsSqlEntity = "IMsSqlEntity";

        public const string RepositorySuffix = "Repository";

        public const string LazySuffix = "LazySubstitute";

        public static readonly string[] CommonUsings =
        {
            "System.Collections.Generic",
            "System.Linq",
            "System"
        };

        public static readonly string[] PartialsUsings =
        {
            "System.Collections.Generic",
            "System.Linq",
            "System",
            "System.Data",
            "System.Text"
        };

        public static readonly string[] OraclePartialsUsings =
        {
            "Oracle.DataAccess.Client"
        };

        public static readonly string[] MsSqlPartialsUsing =
        {
            "System.Data.SqlClient"
        };
    }
}
