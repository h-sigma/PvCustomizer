using System;
using Akaal.Editor;
using UnityEngine;

namespace PvCustomizer.Editor.Samples
{
    public class SampleAssetDrawer : IDrawer
    {
        #region Implementation of IDrawer

        public void Draw(object value, Rect fullRect, bool selected, IconStyle style)
        {
            if (!(value is GridSample asset)) return;
            PvCustomizerGUI.DrawBackground(style.DrawRect);
            PvCustomizerGUI.DrawTexture(style.DrawRect, asset.texture1, style.Material, style.Tint, style.ScaleMode);
        }

        public bool ValidForType(Type type)
        {
            return type.IsAssignableFrom(typeof(GridSample));
        }

        public int Priority => 150;

        #endregion
    }
}