using System;
using Akaal.PvCustomizer.Editor.Utils;
using UnityEngine;

namespace Akaal.PvCustomizer.Editor.DefaultDrawers
{
    public class ColorDrawer : IDrawer
    {
        #region Implementation of IIconDrawer

        public void Draw(object value, Rect fullRect, bool selected, IconStyle style)
        {
            if (!(value is Color color)) return;
            PvCustomizerGUI.DrawColor(style.DrawRect, color * style.Tint);
            if (style.CustomValues.Length > 0)
            {
                var drawText = (bool) style.CustomValues[0];
                if (drawText)
                {
                    PvCustomizerGUI.DrawText(style.DrawRect, color.ToString(), out Rect occupado);
                }
            }
        }

        public bool ValidForType(Type type)
        {
            return typeof(Color).IsAssignableFrom(type);
        }

        public int Priority => PvCustomizerUtility.DefaultPriority;

        #endregion
    }
}