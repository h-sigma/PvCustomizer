using System;
using Akaal.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Akaal.Editor.DefaultDrawers
{
    public class MeshDrawer : IDrawer
    {
        #region Implementation of IIconDrawer

        public void Draw(object value, Rect fullRect, bool selected, IconStyle style)
        {
            if (!(value is Mesh mesh)) return;
            if (!AssetDatabase.Contains(mesh))
            {
                //TODO
            }
            else
            {
                var tex = AssetPreview.GetAssetPreview(mesh);
                PvCustomizerGUI.DrawTexture(style.DrawRect, tex, PvCustomizerGUI.AssetPreviewClipMaterial, style.Tint,
                    style.ScaleMode);
            }
        }

        public bool ValidForType(Type type)
        {
            return type.IsAssignableFrom(typeof(Mesh));
        }

        public int Priority => PvCustomizerUtility.DefaultPriority;

        #endregion
    }
}