using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Battleship.Core.Extensions
{
    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, Func<object>> Instantiators = new Dictionary<Type, Func<object>>();

        /// <summary>
        /// Returns a Func that can be used to create an instance of the given type
        /// </summary>
        public static Func<TNew> GetInstantiator<TNew>(this Type type)
            where TNew : class
        {
            return Expression.Lambda<Func<TNew>>(Expression.New(type)).Compile();
        }

        /// <summary>
        /// Returns a Func that can be used to create an instance of the given type with a single parameter.
        /// Also allowing for an explicit cast before calling the constructor with the parameter.
        /// </summary>
        public static Func<TParam, TNew> GetInstantiator<TParam, TNew>(this Type type, Type parameterType = null)
            where TNew : class
        {
            var constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new[] { parameterType }, null);
            var parameterExpression = Expression.Parameter(typeof(TParam), "arg");
            var castExpression = parameterType == null || typeof(TParam) == parameterType
                ? (Expression)parameterExpression
                : Expression.Convert(parameterExpression, parameterType);

            return Expression.Lambda<Func<TParam, TNew>>(Expression.New(constructor, castExpression), parameterExpression).Compile();
        }

        /// <summary>
        /// Returns an instance of the <paramref name="type"/> on which the method is invoked.
        /// </summary>
        public static object GetInstance(this Type type, bool cache = true)
        {
            if (!cache)
                return Activator.CreateInstance(type);

            if (!Instantiators.TryGetValue(type, out Func<object> instantiator))
                Instantiators[type] = instantiator = Expression.Lambda<Func<object>>(Expression.New(type)).Compile();

            return instantiator.Invoke();
        }

        public static IEnumerable<Type> GetSubTypes(this Type type)
        {
            return type.Assembly.GetTypes().Where(x => x.IsAssignableFrom(type));
        }
    }
}
