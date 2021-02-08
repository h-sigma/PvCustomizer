using System;
using Akaal.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Akaal.Editor.DefaultDrawers
{
    public class MaterialDrawer : IDrawer
    {
        #region Implementation of IIconDrawer

        public void Draw(object value, Rect fullRect, bool selected, IconStyle style)
        {
            if (!(value is Material material)) return;
            if (!AssetDatabase.Contains(material) || material.GetTag("PreviewType", false) == "Plane")
            {
                PvCustomizerGUI.DrawTexture(style.DrawRect, Texture2D.whiteTexture, material, style.Tint,
                    style.ScaleMode);
            }
            else
            {
                var tex = AssetPreview.GetAssetPreview(material);
                PvCustomizerGUI.DrawTexture(style.DrawRect, tex, material, style.Tint, style.ScaleMode);
            }

            /*
             Mesh mesh = PvCustomizerUtility.GetPrimitiveMesh(material.GetTag("PreviewType", false, "Sphere"));

            foreach (Type type in TypeCache.GetTypesDerivedFrom<ObjectPreview>())
            {
                foreach (object attr in type.GetCustomAttributes(typeof(CustomPreviewAttribute), false))
                {
                    if (!(attr is CustomPreviewAttribute cust)) continue;
                    FieldInfo fieldInfo =
                        typeof(CustomPreviewAttribute).GetField("m_Type",
                            BindingFlags.Instance | BindingFlags.NonPublic);
                    Type held = fieldInfo.
                        GetValue(cust) as Type;
                    if (held != typeof(Material)) continue;
                    Debug.Log($"Found material preview: {held.Name}.");

                    var preview = Activator.CreateInstance(held) as ObjectPreview;
                    preview.OnPreviewGUI(style.DrawRect, GUIStyle.none);
                }
            }*/
        }

        public bool ValidForType(Type type)
        {
            return type.IsAssignableFrom(typeof(Material));
        }

        public int Priority => PvCustomizerUtility.DefaultPriority;

        #endregion
    }
}