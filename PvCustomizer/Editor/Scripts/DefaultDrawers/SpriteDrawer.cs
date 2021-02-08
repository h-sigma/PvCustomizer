using System;
using Akaal.Editor.Utils;
using UnityEngine;

namespace Akaal.Editor.DefaultDrawers
{
    public class SpriteDrawer : IDrawer
    {
        #region Implementation of IIconDrawer

        public void Draw(object value, Rect fullRect, bool selected, IconStyle style)
        {
            if (!(value is Sprite sprite)) return;
            PvCustomizerGUI.DrawSprite(style.DrawRect, sprite, style.Material, tint: style.Tint);
        }

        public bool ValidForType(Type type)
        {
            return type.IsAssignableFrom(typeof(Sprite));
        }

        public int Priority => PvCustomizerUtility.DefaultPriority;

        #endregion
    }
}