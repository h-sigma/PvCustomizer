using System.Collections.Generic;
using Akaal.PvCustomizer.Editor.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

[assembly: UxmlNamespacePrefix("Akaal.Editor.EditorGUI", "PvC")]

namespace Akaal.PvCustomizer.Editor.EditorGUI
{
    static class PvFolderQuickAdd
    {
        private static readonly List<Object> _selection = new List<Object>();

        [MenuItem("Window/Deadend/Customize Folder %&c")]
        private static void TryDisplayFolderPanel()
        {
            Object[] sel = Selection.GetFiltered(typeof(Object),
                SelectionMode.Assets | SelectionMode.ExcludePrefab);
            _selection.Clear();
            foreach (Object o in sel)
            {
                if (AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(o))) _selection.Add(o);
            }

            if (_selection.Count == 0) return;
            var ruleOptionsWindow = EditorWindow.GetWindow<FolderRuleOptionsWindow>(true, "Folder Rule");
            ruleOptionsWindow.SetSelection(_selection);
        }
    }

    public class FolderRuleOptionsWindow : EditorWindow
    {
        private PvRuleItem     currentRule;
        private Object         currentObject;
        private SerializedRule currentSrlRule;
        private int            currentSelection;

        private int CurrentSelection
        {
            get => currentSelection;
            set
            {
                currentSelection                             = value;
                (currentObject, currentRule, currentSrlRule) = _matches[value];
            }
        }

        private FolderRuleOptionsPanel _optionsPanel;

        private List<(Object, PvRuleItem, SerializedRule)> _matches =
            new List<(Object, PvRuleItem, SerializedRule)>();

        private List<PvRuleItem> _cachedList = new List<PvRuleItem>();
        private Button           _nextButton;
        private Button           _prevButton;
        private Button           _newRuleButton;

        public void SetSelection(List<Object> selection)
        {
            rootVisualElement.Clear();
            rootVisualElement.Unbind();

            _matches.Clear();

            FillMatchesList(selection);
            currentSelection = -1;

            Resource.Load<VisualTreeAsset>("UXML/FolderRuleOptionsPanel_UXML.uxml").CloneTree(rootVisualElement);
            rootVisualElement.style.flexGrow = 1;

            _optionsPanel = rootVisualElement.Q<FolderRuleOptionsPanel>();

            _optionsPanel.OnCancel   = OnCancel;
            _optionsPanel.OnReset    = OnReset;
            _optionsPanel.OnSettings = OnSettings;
            _optionsPanel.OnApply    = OnApply;
            _optionsPanel.OnDelete   = OnDelete;

            var buttonHolder = _optionsPanel.btn_Delete.parent;

            const string className = "icon-button";
            _nextButton    = new Button(OnNext) {name       = "btn_Next", text     = null}.AddClass(className);
            _prevButton    = new Button(OnPrevious) {name   = "btn_Previous", text = null}.AddClass(className);
            _newRuleButton = new Button(OnAddNewRule) {name = "btn_NewRule", text  = null}.AddClass(className);
            buttonHolder.Add(_prevButton);
            buttonHolder.Add(_nextButton);
            buttonHolder.Add(_newRuleButton);
            OnNext();

            rootVisualElement.schedule.Execute(UpdateStateFrequent).Every(200);
        }

        #region Next/Previous/AddNew Buttons

        private void OnPrevious()
        {
            CurrentSelection = Mathf.Clamp(currentSelection - 1, 0, _matches.Count - 1);
            UpdateState();
        }

        private void OnAddNewRule()
        {
            Object o = currentObject;
            PvRuleItem rule = new PvRuleItem()
            {
                enabled = true, ruleString = o.name, ruleType = PvRuleType.Name
            };
            _matches.Insert(currentSelection + 1, (o, rule, SerializedRule.FromRule(rule)));
            OnNext();
        }

        private void OnNext()
        {
            CurrentSelection = Mathf.Clamp(currentSelection + 1, 0, _matches.Count - 1);
            UpdateState();
        }

        private void UpdateState()
        {
            if (CurrentSelection >= _matches.Count)
            {
                Close();
                return;
            }

            UpdateStateFrequent();

            _optionsPanel.SetCurrentBound(currentSrlRule, currentObject);
        }

        private void UpdateStateFrequent()
        {
            _prevButton.SetEnabled(CurrentSelection > 0);
            _nextButton.SetEnabled(CurrentSelection < _matches.Count - 1);
            Label tagMod = _optionsPanel.tag_Modified;
            if (currentRule.Equals(currentSrlRule.rule))
            {
                if (tagMod.parent != null)
                    tagMod.RemoveFromHierarchy();
            }
            else if (tagMod.parent == null)
            {
                _optionsPanel.tagHolder.Add(tagMod);
            }

            Label tagNew = _optionsPanel.tag_New;
            if (PvCustomizerSettings.GetOrCreateSettings().Rules.Exists(m => object.ReferenceEquals(m, currentRule)))
            {
                if (tagNew.parent != null)
                    tagNew.RemoveFromHierarchy();
            }
            else if (tagNew.parent == null)
            {
                _optionsPanel.tagHolder.Add(tagNew);
            }
        }

        #endregion

        public void FillMatchesList(List<Object> selection)
        {
            var settings = PvCustomizerSettings.GetOrCreateSettings();

            foreach (Object o in selection)
            {
                _cachedList.Clear();
                int matched = settings.GetAllMatchingRules(AssetDatabase.GetAssetPath(o), _cachedList, false);

                if (matched == 0)
                {
                    var rule = new PvRuleItem()
                    {
                        enabled    = true,
                        ruleType   = PvRuleType.Name,
                        ruleString = o.name
                    };
                    _matches.Add((o, rule, SerializedRule.FromRule(rule)));
                }
                else
                {
                    foreach (PvRuleItem ruleItem in _cachedList)
                    {
                        _matches.Add((o, ruleItem, SerializedRule.FromRule(ruleItem)));
                    }
                }
            }
        }

        private void OnSettings()
        {
        }

        private void OnDelete()
        {
            bool confirmation = EditorUtility.DisplayDialog("Confirmation",
                "Are you sure you wish to delete this rule?",
                "Delete",
                "Cancel");
            if (!confirmation) return;
            var settings = PvCustomizerSettings.GetOrCreateSettings();
            settings.Rules.RemoveAll(r => ReferenceEquals(r, currentRule));
            _matches.RemoveAll(m => ReferenceEquals(m.Item2, currentRule));
            OnNext();
        }

        private void OnReset()
        {
            bool confirmation = EditorUtility.DisplayDialog("Confirmation",
                "Are you sure you wish to reset this rule?",
                "Reset",
                "Cancel");
            if (!confirmation) return;
            var temp = currentSrlRule.rule;
            temp.CopyFrom(currentRule);
            UpdateState();
        }

        private void OnCancel()
        {
            Close();
        }

        private void OnApply()
        {
            currentRule.CopyFrom(currentSrlRule.rule);
            var settings = PvCustomizerSettings.GetOrCreateSettings();
            if (!settings.Rules.Contains(currentRule)) settings.Rules.Add(currentRule);
            UpdateState();
        }
    }
}