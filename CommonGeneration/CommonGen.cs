namespace ContextItems.CommonGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class CommonGen
    {
        private static readonly Dictionary<Type, string> TypeAliases = new Dictionary<Type, string>
                                                                       {
                                                                           { typeof(char), "char" },
                                                                           { typeof(byte[]), "byte[]" },
                                                                           { typeof(byte), "byte" },
                                                                           { typeof(long), "long" },
                                                                           { typeof(int), "int" },
                                                                           { typeof(decimal), "decimal" },
                                                                           { typeof(string), "string" },
                                                                           { typeof(short), "short" },
                                                                           { typeof(bool), "bool" }
                                                                       };

        public static string GetTypeAliasName(Type type)
        {
            return TypeAliases.ContainsKey(type) ? TypeAliases[type] : type.Name;
        }

        public static string GetTypeName(this Type type)
        {
            return IsNullable(type) ? (GetTypeAliasName(GenArgument(type)) + "?") : GetTypeAliasName(type);
        }

        public static bool IsFieldNullableType(Type mappedType, string fieldName)
        {
            return GetFieldType(mappedType, fieldName).IsNullable();
        }

        public static Type GetFieldType(Type type, string fieldName)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public).First(x => x.Name == fieldName).PropertyType;
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsDefinedGeneric(typeof(Nullable<>));
        }

        public static bool IsDefinedGeneric(this Type type, Type genType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genType;
        }

        public static Type GenArgument(this Type type)
        {
            return type.GetGenericArguments().First();
        }
    }
}