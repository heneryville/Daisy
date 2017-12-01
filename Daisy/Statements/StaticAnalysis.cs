namespace Ancestry.Daisy.Statements
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class StaticAnalysis
    {
        public class ProceedFunction
        {
            public Type ChildScopeType { get; set; }
        }

        public static bool IsStatementController(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(StatementController<>)) return true;
            return type.BaseType != null && IsStatementController(type.BaseType);
        }

        public static bool IsStatementMethod(MethodInfo m)
        {
            return m.IsPublic
                && m.ReturnType == typeof(bool) && m.DeclaringType != typeof(object);
        }

        public static bool IsAggregateMethod(MethodInfo m)
        {
            return m.IsPublic && ExtractProceedFunction(m) != null;
        }

        public static bool IsProceedFunction(Type func)
        {
            return ExtractProceedFunctionType(func) != null;
        }

        public static Type ExtractProceedFunctionType(Type func)
        {
            if (func.BaseType != typeof(MulticastDelegate)) return null;
            if (!func.IsGenericType) return null;
            var gs = func.GenericTypeArguments;
            if (gs.Length != 2) return null;
            if (gs[1] != typeof(bool)) return null;
            return gs[0];
        }

        public static ParameterInfo ExtractProceedFunction(MethodInfo m)
        {
            return m.GetParameters()
                .LastOrDefault(p => IsProceedFunction(p.ParameterType))
                ;
        }

        /// <summary>
        /// Converts a predicate of Func<object,bool> to
        /// Func<Type,bool> of the given type.
        /// </summary>
        /// <param name="destType">Type of the dest.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static TransformPredicate CreateConverter(Type destType)
        {
            // This essentially creates the following lambda, but uses destType instead of T
            // private static Func<Func<object, bool>, Func<T, bool>> Transform<T>()
            // { 
            //     return (Func<object,bool> input) => ((T x) => input(x));
            // }
            var input = Expression.Parameter(typeof(Func<object, bool>), "input");

            var x = Expression.Parameter(destType, "x");
            var convert = Expression.Convert(x, typeof(object));
            var callInputOnX = Expression.Invoke(input, convert);
            var body2 = Expression.Lambda(callInputOnX, x);
            var body1 = Expression.Lambda(typeof(TransformPredicate),body2, input);
            var compiled = body1.Compile();
            return (TransformPredicate)compiled;
            /*
            var destFunc = typeof(Func<,>).MakeGenericType(destType, typeof(bool));
            var endType = typeof(Func<,>).MakeGenericType(typeof(Func<object, bool>), destFunc);
            return (TransformPredicate)compiled.Method.CreateDelegate(endType);
            */
        }

        public delegate object TransformPredicate(Func<object,bool> weak);

    }
}
