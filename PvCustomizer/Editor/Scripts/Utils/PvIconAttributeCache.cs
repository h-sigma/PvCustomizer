using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Akaal.Editor.Utils
{
    /// <summary>
    /// Keeps a cache of a type's Valueable members that also have the PvIconAttribute declared on them.
    /// </summary>
    public static class PvIconAttributeCache
    {
        private static readonly Dictionary<Type, List<Triplet>> s_MemberAttrCache =
            new Dictionary<Type, List<Triplet>>();

        private static readonly Dictionary<Type, PvIconAttribute[]> s_ClassAttrCache =
            new Dictionary<Type, PvIconAttribute[]>();

        public struct Triplet
        {
            public MemberInfo      Member;
            public PvIconAttribute Attr;
            public Type            ValueType;

            public void Deconstruct(out MemberInfo m, out PvIconAttribute attr, out Type type)
            {
                m    = Member;
                attr = Attr;
                type = ValueType;
            }
        }

        public static List<Triplet> GetAttributeTriplets(Type type)
        {
            //if we have it cached, viola
            if (s_MemberAttrCache.TryGetValue(type, out var allAttrs)) return allAttrs;

            allAttrs = new List<Triplet>();

            //from all members that can easily yield a value,
            //filter out the ones with one or more relevant attributes, and add to cache
            foreach (var valueable in ValueableMemberCache.GetValueables(type))
            {
                foreach (var projectIconAttribute in valueable.Member.GetCustomAttributes<PvIconAttribute>())
                {
                    allAttrs.Add(new Triplet()
                        {Attr = projectIconAttribute, Member = valueable.Member, ValueType = valueable.ValueType});
                }
            }

            //sort in order of layers to ensure correct draw order, shouldn't need to re-order again
            allAttrs.Sort((a, b) => a.Attr.Layer.CompareTo(b.Attr.Layer));

            //assign to cache
            s_MemberAttrCache[type] = allAttrs;

            return allAttrs;
        }

        public static bool TryGetClassAttribute(Type type, out PvIconAttribute[] attr)
        {
            if (s_ClassAttrCache.TryGetValue(type, out attr)) return attr.Length > 0;

            attr                   = type.GetCustomAttributes<PvIconAttribute>().ToArray();
            s_ClassAttrCache[type] = attr;
            return attr.Length > 0;
        }
    }
}