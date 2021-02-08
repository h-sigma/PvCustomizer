using System;
using System.IO;
using Akaal.Editor.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Akaal.Editor.EditorGUI
{
    public class FolderRuleOptionsPanel : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<FolderRuleOptionsPanel, UxmlTraits>
        {
        }

        #region Fetches

        [Fetch(className = "config")]
        public VisualElement ruleConfig;

        [Fetch("ruleType")]
        public EnumField ruleType;

        [Fetch("ruleString")]
        public TextField ruleString;

        [Fetch(className = "header__enabled")]
        public Toggle enabledToggle;

        //object fields
        [Fetch(name = "largeIcon__field")]
        public ObjectField largeIconField;

        [Fetch(name = "smallIcon__field")]
        public ObjectField smallIconField;

        [Fetch(name = "textBackground__field")]
        public ObjectField textBackgroundField;

        [Fetch(name = "largeIcon__display")]
        public VisualElement largeIconDisplay;

        [Fetch(name = "smallIcon__display")]
        public VisualElement smallIconDisplay;

        [Fetch(name = "textBackground__display")]
        public VisualElement textBackgroundDisplay;

        [Fetch(name = nameof(btn_Apply))]
        private Button btn_Apply;

        [Fetch(name = nameof(btn_Settings))]
        public Button btn_Settings;

        [Fetch(name = nameof(btn_Cancel))]
        public Button btn_Cancel;

        [Fetch(name = nameof(btn_Reset))]
        public Button btn_Reset;

        [Fetch(name = nameof(btn_Delete))]
        public Button btn_Delete;

        [Fetch(name = nameof(tag_Modified))]
        public Label tag_Modified;

        [Fetch(name = nameof(tag_New))]
        public Label tag_New;

        [Fetch("tag-holder")]
        public VisualElement tagHolder;

        [Fetch(nameof(textColor))]
        public ColorField textColor;

        [Fetch(nameof(sampleText))]
        public Label sampleText;

        #endregion

        public SerializedRule TempRule;

        public Object folderAsset;

        public Action OnApply;
        public Action OnCancel;
        public Action OnSettings;
        public Action OnReset;
        public Action OnDelete;

        public FolderRuleOptionsPanel()
        {
            this.RegisterCallback<AttachToPanelEvent>((evt) =>
                FetchCallback.OnAttachedToPanel(evt, OnFetchCallbackFinish));
        }

        private void OnFetchCallbackFinish()
        {
            //todo -- unsubscribe
            ruleConfig?.SetEnabled(enabledToggle.value);
            enabledToggle?.UnregisterValueChangedCallback(onEnableToggled);
            enabledToggle?.RegisterValueChangedCallback(onEnableToggled);
            FieldAndIconPair(largeIconField,      largeIconDisplay);
            FieldAndIconPair(smallIconField,      smallIconDisplay);
            FieldAndIconPair(textBackgroundField, textBackgroundDisplay);

            ruleType.RegisterValueChangedCallback(OnRuleTypeChanged);

            if (btn_Apply    != null) btn_Apply.clickable.clicked    += () => OnApply?.Invoke();
            if (btn_Cancel   != null) btn_Cancel.clickable.clicked   += () => OnCancel?.Invoke();
            if (btn_Reset    != null) btn_Reset.clickable.clicked    += () => OnReset?.Invoke();
            if (btn_Settings != null) btn_Settings.clickable.clicked += () => OnSettings?.Invoke();
            if (btn_Delete   != null) btn_Delete.clickable.clicked   += () => OnDelete?.Invoke();

            SetSampleTextColor(textColor.value);
            textColor.RegisterValueChangedCallback(evt => { SetSampleTextColor(evt.newValue); });

            void SetSampleTextColor(Color color)
            {
                sampleText.style.color = color;
            }
        }

        private void OnRuleTypeChanged(ChangeEvent<Enum> evt)
        {
            if (evt.previousValue == evt.newValue) return;
            string assetPath = AssetDatabase.GetAssetPath(folderAsset);
            switch (evt.newValue)
            {
                case PvRuleType.Name:
                    TempRule.rule.ruleString = Path.GetFileName(assetPath);
                    break;
                case PvRuleType.Extension:
                    TempRule.rule.ruleString = Path.GetExtension(assetPath);
                    break;
                case PvRuleType.Path:
                    TempRule.rule.ruleString = assetPath;
                    break;
                case PvRuleType.Regex:
                    TempRule.rule.ruleString = assetPath;
                    break;
            }

            ruleString.SetValueWithoutNotify(TempRule.rule.ruleString);
        }

        private void onEnableToggled(ChangeEvent<bool> evt)
        {
            ruleConfig?.SetEnabled(evt.newValue);
        }

        private static void FieldAndIconPair(ObjectField iconField, VisualElement iconDisplay)
        {
            iconField.objectType = typeof(Sprite);
            iconField.RegisterValueChangedCallback(evt =>
                StyleBackgroundImage(iconDisplay, evt.newValue));

            StyleBackgroundImage(iconDisplay, iconField.value);

            void StyleBackgroundImage(VisualElement appliedTo, Object sprite)
            {
                appliedTo.style.backgroundImage =
                    new StyleBackground((sprite as Sprite).AsUnityNull()?.texture);
            }
        }

        public void SetCurrentBound(SerializedRule temp, Object o)
        {
            folderAsset = o;
            TempRule    = temp;
            this.Bind(new SerializedObject(TempRule));
        }
    }

    public class SerializedRule : ScriptableObject
    {
        public PvRuleItem rule;

        public static SerializedRule FromRule(PvRuleItem rule)
        {
            var srlRule = CreateInstance<SerializedRule>();
            srlRule.rule = rule.ShallowCopy();
            return srlRule;
        }
    }
}