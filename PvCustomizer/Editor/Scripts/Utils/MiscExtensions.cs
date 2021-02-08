using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Akaal.Editor.Utils
{
    internal static class MiscExtensions
    {
        public static int GetAllMatchingRules(this PvCustomizerSettings settings, string path,
            List<PvRuleItem> matchingRules, bool enabledOnly = true)
        {
            int matched = 0;
            for (int i = 0; i < settings.Rules.Count; i++)
            {
                PvRuleItem rule = settings.Rules[i];
                if (!rule.enabled && enabledOnly) continue;

                if (DoesPathMatchRule(path, rule))
                {
                    matched++;
                    matchingRules.Add(rule);
                }
            }

            return matched;
        }

        public static T AddClass<T>(this T element, string className) where T : VisualElement
        {
            element.AddToClassList(className);
            return element;
        }

        public static bool TryMatchAgainstRules(this PvCustomizerSettings settings, string path,
            out PvRuleItem matchingRule, bool enabledOnly = true, bool respectPriority = true)
        {
            int matchIndex = -1;
            for (int i = 0; i < settings.Rules.Count; i++)
            {
                PvRuleItem rule = settings.Rules[i];
                if (!rule.enabled && enabledOnly) continue;
                bool didMatch = false;
                didMatch = DoesPathMatchRule(path, rule);

                if (didMatch)
                {
                    if (matchIndex == -1 || settings.Rules[matchIndex].priority < settings.Rules[i].priority)
                        matchIndex = i;
                    if (!respectPriority) break;
                }
            }

            if (matchIndex == -1)
            {
                matchingRule = null;
                return false;
            }
            else
            {
                matchingRule = settings.Rules[matchIndex];
                return true;
            }
        }

        private static bool DoesPathMatchRule(string path, PvRuleItem rule)
        {
            bool didMatch = false;
            switch (rule.ruleType)
            {
                case PvRuleType.Name:
                    string folderName = Path.GetFileName(path);

                    if (rule.ruleString == folderName) didMatch = true;
                    break;
                case PvRuleType.Path:
                    if (rule.ruleString == path) didMatch = true;
                    break;
                case PvRuleType.Extension:
                    string extn                           = Path.GetExtension(path);
                    if (extn == rule.ruleString) didMatch = true;
                    break;
                case PvRuleType.Regex:
                    if (Regex.IsMatch(path, rule.ruleString)) didMatch = true;
                    break;
            }

            return didMatch;
        }

        public static string FirstCharacterToLower(this string s)
        {
            if (string.IsNullOrEmpty(s) || char.IsLower(s, 0))
            {
                return s;
            }

            return char.ToLowerInvariant(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// Converts Unity pseudo-null to real null, allowing for coallescing operators.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T AsUnityNull<T>(this T obj) where T : Object
        {
            if (obj == null) return null;
            return obj;
        }

        public static Vector2 Size(this Texture2D tex) => new Vector2(tex.width, tex.height);

        /// <summary>
        /// Calculates aspect ratio of a Rect as width divided by height.
        /// </summary>
        public static float Aspect(this Rect rect) => rect.width / rect.height;

        /// <summary>
        /// Tries to get a value from a MemberInfo and object with type defining the member.
        /// Supports fields, readable properties, and parameter-less non-void-return methods.
        /// </summary>
        /// <param name="definingObject">The object with type that defines the member.</param>
        /// <returns>Value if member is one of suitable FieldInfo, MethodInfo, PropertyInfo. Null otherwise.</returns>
        public static object GetMemberValue(this MemberInfo member, object definingObject)
        {
            switch (member)
            {
                case FieldInfo fieldInfo:       return fieldInfo.GetValue(definingObject);
                case PropertyInfo propertyInfo: return propertyInfo.GetValue(definingObject);
                case MethodInfo methodInfo:     return methodInfo.Invoke(definingObject, Array.Empty<object>());
                default:                        return null;
            }
        }

        public static UnityEngine.ScaleMode UnityScaleMode(this PvScaleMode iconScaleMode)
        {
            UnityEngine.ScaleMode scaleMode = (UnityEngine.ScaleMode) iconScaleMode;
            return scaleMode;
        }

        public static TextAnchor UnityTextAnchor(this PvAnchor anchor)
        {
            return (TextAnchor) anchor;
        }

        public static UnityEngine.FontStyle UnityFontStyle(this UnityEngine.FontStyle style)
        {
            return (UnityEngine.FontStyle) style;
        }
    }
}