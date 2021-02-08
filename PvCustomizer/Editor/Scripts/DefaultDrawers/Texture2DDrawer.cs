using System;
using Akaal.Editor.Utils;
using UnityEngine;

namespace Akaal.Editor.DefaultDrawers
{
    public class Texture2DDrawer : IDrawer
    {
        #region Implementation of IIconDrawer

        public void Draw(object value, Rect fullRect, bool selected, IconStyle style)
        {
            if (!(value is Texture2D tex)) return;
            PvCustomizerGUI.DrawTexture(style.DrawRect, tex, style.Material, style.Tint, style.ScaleMode);
        }

        public bool ValidForType(Type type)
        {
            return type.IsAssignableFrom(typeof(Texture2D));
        }

        public int Priority => PvCustomizerUtility.DefaultPriority;

        #endregion
    }
}