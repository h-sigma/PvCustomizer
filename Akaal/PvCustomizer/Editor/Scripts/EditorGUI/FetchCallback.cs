using System;
using System.Reflection;
using UnityEngine.UIElements;

namespace Akaal.PvCustomizer.Editor.EditorGUI
{
    public static class FetchCallback
    {
        public static void OnAttachedToPanel(AttachToPanelEvent evt, Action action)
        {
            OnAttachedToPanel(evt);
            action?.Invoke();
        }

        public static void OnAttachedToPanel(AttachToPanelEvent evt)
        {
            if (evt.propagationPhase != PropagationPhase.AtTarget || !(evt.target is VisualElement root)) return;
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var          fields       = root.GetType().GetFields(bindingFlags);
            foreach (var field in fields)
            {
                foreach (var attribute in field.GetCustomAttributes<FetchAttribute>(false))
                {
                    if (attribute is FetchAttribute fetch)
                    {
                        var fieldType = field.FieldType;
                        var query     = root.Query(name: fetch.name, className: fetch.className);
                        var queried = query.Where(item => fieldType.IsInstanceOfType(item)).First();

                        if (queried is IBindable bindable && !string.IsNullOrEmpty(fetch.bindingPath))
                        {
                            bindable.bindingPath = fetch.bindingPath;
                        }

                        field.SetValue(root, queried);
                    }
                }
            }
        }
    }
}