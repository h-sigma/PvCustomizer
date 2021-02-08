using System;
using Akaal.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Akaal.Editor.DefaultDrawers
{
    public class AudioClipDrawer : IDrawer
    {
        #region Implementation of IIconDrawer

        public void Draw(object value, Rect fullRect, bool selected, IconStyle style)
        {
            if (!(value is AudioClip clip)) return;
            if (!AssetDatabase.Contains(clip))
            {
                //TODO
            }
            else
            {
                var tex = AssetPreview.GetAssetPreview(clip);
                PvCustomizerGUI.DrawTexture(style.DrawRect, tex,PvCustomizerGUI.AssetPreviewClipMaterial, style.Tint, style.ScaleMode);
            }
        }

        public bool ValidForType(Type type)
        {
            return type.IsAssignableFrom(typeof(AudioClip));
        }

        public int Priority => PvCustomizerUtility.DefaultPriority;

        #endregion
    }
}