using System;
using System.Collections.Generic;
using System.Reflection;

namespace Akaal.PvCustomizer.Editor.Utils
{
    /// <summary>
    /// Keeps a cache of a type's members that can "easily" yield a value.
    /// This means fields, properties with a getter, and parameter-less methods with non-void return.
    /// </summary>
    public static class ValueableMemberCache
    {
        private static readonly Dictionary<Type, List<Pair>> s_Cache = new Dictionary<Type, List<Pair>>();

        public struct Pair
        {
            public MemberInfo Member;
            public Type       ValueType;
        }

        public static List<Pair> GetValueables(Type type)
        {
            if (s_Cache.TryGetValue(type, out var pairs)) return pairs;

            pairs = new List<Pair>();

            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (FieldInfo fieldInfo in type.GetFields(flags))
            {
                pairs.Add(new Pair() {Member = fieldInfo, ValueType = fieldInfo.FieldType});
            }

            //check all readable properties
            foreach (PropertyInfo propertyInfo in type.GetProperties(flags))
            {
                if (!propertyInfo.CanRead) continue;
                pairs.Add(new Pair() {Member = propertyInfo, ValueType = propertyInfo.PropertyType});
            }

            //check all parameter-less, non abstract methods without void return
            foreach (MethodInfo methodInfo in type.GetMethods(flags))
            {
                if (methodInfo.ReturnType             == typeof(void) || methodInfo.IsAbstract ||
                    methodInfo.GetParameters().Length > 0) continue;
                pairs.Add(new Pair() {Member = methodInfo, ValueType = methodInfo.ReturnType});
            }

            s_Cache[type] = pairs;

            return pairs;
        }
    }
}