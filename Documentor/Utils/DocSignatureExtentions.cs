namespace Ancestry.Daisy.Documentation.Utils
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal static class DocSignatureExtentions
    {
        internal static string GetDocStyleSignature(this MethodInfo method)
        {
            var parameters = method.GetParameters().Select(x => GetDocStyleSignature(x.ParameterType)).ToList();
            var parameterClause = "(" + string.Join(",", parameters) + ")";
            return method.DeclaringType.FullName + "." + method.Name
                + (parameters.Count > 0 ? parameterClause : "");
        }

        internal static string GetDocStyleSignature(this Type type)
        {
            var nameClause = type.Namespace + "." + NormalizeTypeName(type.Name);
            var genericPart = "{" + string.Join(",", type.GetGenericArguments().Select(GetDocStyleSignature)) + "}";
            return nameClause + (genericPart.Length == 2 ? "" : genericPart);
        }

        private static string NormalizeTypeName(string name)
        {
            var arityMark = name.IndexOf('`');
            if (arityMark > -1) name = name.Substring(0, arityMark);
            return name;
        }
    }
}
