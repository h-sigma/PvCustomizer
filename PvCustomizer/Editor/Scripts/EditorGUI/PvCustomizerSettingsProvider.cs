using System.IO;
using Akaal.Editor.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Akaal.Editor.EditorGUI
{
    class PvCustomizerSettingsProvider : SettingsProvider
    {
        private SerializedObject        _srlSettings;
        private IMGUIContainer          _tintExampleContainer;
        private PvCustomizerSettings    _settings;
        private UQueryBuilder<ListView> _rulesListQ;
        private Sprite                  _folderExampleSprite;

        public PvCustomizerSettingsProvider(string path, SettingsScope scope = SettingsScope.User)
            : base(path, scope)
        {
        }

        public static bool IsSettingsAvailable()
        {
            return File.Exists(PvCustomizerSettings.k_MyCustomSettingsPath);
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            // This function is called when the user clicks on the MyCustom element in the Settings window.
            _settings    = PvCustomizerSettings.GetOrCreateSettings();
            _srlSettings = new SerializedObject(_settings);
            keywords     = GetSearchKeywordsFromSerializedObject(_srlSettings);

            var tree = Resource.Load<VisualTreeAsset>("UXML/PvCustomizerSettingsProvider_UXML.uxml");
            tree.CloneTree(rootElement);

            #region images

            rootElement.Q(className: "logo").style.backgroundImage =
                new StyleBackground(Resource.Load<Texture2D>("Icons/logo.png"));

            #endregion

            #region list-view

            _rulesListQ = rootElement.Query<ListView>().Name("rulesList");
            ListView rulesList = _rulesListQ.First();
            rulesList.bindItem = (vi, i) =>
            {
                FolderRuleOptionsPanel optionsPanel = vi.Q<FolderRuleOptionsPanel>();
                if (i >= 0 && _settings.Rules.Count > i)
                {
                    optionsPanel.SetEnabled(true);
                    SerializedRule serializedRule = ScriptableObject.CreateInstance<SerializedRule>();
                    serializedRule.rule = _settings.Rules[i];
                    optionsPanel.SetCurrentBound(serializedRule, null);
                }
                else
                {
                    optionsPanel.SetEnabled(false);
                }
            };

            rulesList.makeItem = () =>
            {
                var child = Resource.Load<VisualTreeAsset>("UXML/FolderRuleOptionsPanel_UXML.uxml").CloneTree();
                child.style.paddingTop        = 5;
                child.style.paddingBottom     = 5;
                child.style.borderBottomWidth = 3;
                child.style.borderBottomColor = Color.black;

                var optionsPanel = child.Q<FolderRuleOptionsPanel>();

                optionsPanel.Q<Button>("btn_Reset")?.RemoveFromHierarchy();
                optionsPanel.Q<Button>("btn_Cancel")?.RemoveFromHierarchy();
                optionsPanel.Q<Button>("btn_Apply")?.RemoveFromHierarchy();
                optionsPanel.Q<Button>("btn_Settings")?.RemoveFromHierarchy();
                optionsPanel.Q("tag-holder").RemoveFromHierarchy();

                optionsPanel.OnDelete = () => DeleteRule(child);

                return child;
            };
            rulesList.style.flexGrow = 1;
            rulesList.Refresh();

            #endregion

            #region tint example + imgui

            _tintExampleContainer              =  rootElement.Q<IMGUIContainer>("tintExample");
            _tintExampleContainer.onGUIHandler += DrawTintExample;

            #endregion

            rootElement.Bind(_srlSettings);
        }

        private void DeleteRule(VisualElement child)
        {
            var optionsPanel = (child as FolderRuleOptionsPanel) ?? child.Q<FolderRuleOptionsPanel>() ??
                child.GetFirstAncestorOfType<FolderRuleOptionsPanel>();
            if (optionsPanel == null) return;
            var boundRule = optionsPanel.TempRule.rule;
            _settings.Rules.RemoveAll(r => ReferenceEquals(boundRule, r));
            _srlSettings.Update();
            ListView listView = _rulesListQ.First();
            listView?.Refresh();
        }

        private void DrawTintExample()
        {
            if (_folderExampleSprite == null)
                _folderExampleSprite = Resource.Load<Sprite>("Icons/Game Elements/008-castle.png");
            Vector2 localBoundSize = _tintExampleContainer.localBound.size;
            Rect    drawRect       = GUILayoutUtility.GetRect(localBoundSize.x, localBoundSize.y);
            PvCustomizerGUI.DrawSprite(drawRect, _folderExampleSprite, tint: PvCustomizerGUI.ICON_SELECTED_TINT);
        }

        #region Overrides of SettingsProvider

        public override void OnDeactivate()
        {
            (PvCustomizerSettings.GetOrCreateSettings())?.RecreateDicts();
            base.OnDeactivate();
        }

        #endregion

        // Register the SettingsProvider
        [SettingsProvider]
        public static SettingsProvider CreatePvCustomizerSettingsProvider()
        {
            if (!IsSettingsAvailable())
            {
                PvCustomizerSettings.GetOrCreateSettings();
            }

            if (IsSettingsAvailable())
            {
                var provider = new PvCustomizerSettingsProvider("PvCustomizer", SettingsScope.User);
                return provider;
            }

            // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
            return null;
        }
    }
}