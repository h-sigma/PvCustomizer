using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Akaal.Editor
{
    public class PvCustomizerSettings : ScriptableObject, ISerializationCallbackReceiver
    {
        public const string PackageName = "com.akaal.pvcustomizer";

        public const string k_MyCustomSettingsPath =
            "Packages/" + PackageName + "/PvCustomizer/Editor/Resources/PvCustomizerSettings.asset";

        #region Definitions

        #endregion

        [SerializeField]
        private float tintAmount;

        [SerializeField]
        private List<PvRuleItem> rules = new List<PvRuleItem>();

        private        Dictionary<string, PvRuleItem> _rulesDict = new Dictionary<string, PvRuleItem>();
        private static PvCustomizerSettings           _settings;
        public         List<PvRuleItem>               Rules => rules;

        public float TintAmount => tintAmount;

        public Dictionary<string, PvRuleItem> RulesDict => _rulesDict;

        #region Implementation of ISerializationCallbackReceiver

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            TrimEmptyRules();
            RecreateDicts();
        }

        private void TrimEmptyRules()
        {
            rules.RemoveAll(r => r.IsEmpty());
        }

        public void RecreateDicts()
        {
            _rulesDict = new Dictionary<string, PvRuleItem>();
            foreach (var pattern in rules)
            {
                if (pattern.IsEmpty()) continue;
                _rulesDict[pattern.ruleString] = pattern;
            }
        }

        #endregion

        #region Static

        internal static PvCustomizerSettings GetOrCreateSettings()
        {
            if (_settings == null)
            {
                _settings = AssetDatabase.LoadAssetAtPath<PvCustomizerSettings>(k_MyCustomSettingsPath);
                if (_settings == null)
                {
                    _settings = CreateInstance<PvCustomizerSettings>();
                    AssetDatabase.CreateAsset(_settings, k_MyCustomSettingsPath);
                    AssetDatabase.SaveAssets();
                }
            }

            return _settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }

        #endregion
    }

    public enum PvRuleType
    {
        Name,
        Path,
        Extension,
        Regex
    }

    [Serializable]
    public struct PvGraphicConfig : IEquatable<PvGraphicConfig>
    {
        public Sprite sprite;
        public Color  color;
        public int    preconfigId;

        #region Implementation of IEquatable<PvGraphicConfig>

        public bool Equals(PvGraphicConfig other)
        {
            return sprite == other.sprite && color == other.color && preconfigId == other.preconfigId;
        }

        #endregion
    }

    [Serializable]
    public class PvRuleItem : IEquatable<PvRuleItem>
    {
        public bool enabled = true;

        public int priority = 0;

        public PvRuleType ruleType = PvRuleType.Name;

        public string ruleString;

        public bool            eraseDefaultFolder = true;
        public PvGraphicConfig smallIcon;

        public PvGraphicConfig largeIcon;

        public PvGraphicConfig textBackground;
        public Color           textColor = new Color(0, 0, 0, 1);

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(ruleString);
        }

        #region Implementation of IEquatable<PvRuleItem>

        public bool Equals(PvRuleItem other)
        {
            if (other == null) return false;
            return enabled = other.enabled && ruleType == other.ruleType && ruleString == other.ruleString &&
                             smallIcon.Equals(other.smallIcon) && largeIcon.Equals(other.largeIcon) &&
                             textBackground.Equals(other.textBackground) && priority == other.priority &&
                             other.textColor == textColor;
        }

        #endregion

        public PvRuleItem ShallowCopy()
        {
            return (PvRuleItem) this.MemberwiseClone();
        }

        public PvRuleItem CopyFrom(PvRuleItem other)
        {
            priority           = other.priority;
            ruleType           = other.ruleType;
            ruleString         = other.ruleString;
            enabled            = other.enabled;
            smallIcon          = other.smallIcon;
            largeIcon          = other.largeIcon;
            textBackground     = other.textBackground;
            textColor          = other.textColor;
            eraseDefaultFolder = other.eraseDefaultFolder;
            return this;
        }
    }
}