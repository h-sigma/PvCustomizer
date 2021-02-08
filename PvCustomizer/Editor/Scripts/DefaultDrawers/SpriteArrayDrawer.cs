using System;
using System.Collections.Generic;
using Akaal.Editor.Utils;
using UnityEngine;

namespace Akaal.Editor.DefaultDrawers
{
    public class SpriteArrayDrawer : IDrawer
    {
        #region Implementation of IIconDrawer

        public void Draw(object value, Rect fullRect, bool selected, IconStyle style)
        {
            if (!(value is IList<Sprite> sprites)) return;

            Rect drawRect = style.DrawRect;
            if (sprites.Count == 0)
            {
                PvCustomizerGUI.DrawBorder(drawRect, Color.magenta);
                PvCustomizerGUI.DrawTextDirect(drawRect, "Empty Array",
                    Color.magenta, UnityEngine.FontStyle.Normal, PvAnchor.MiddleCenter);
            }
            else
            {
                int   gridSize = Mathf.CeilToInt(Mathf.Sqrt(sprites.Count));
                float width    = drawRect.width  / gridSize;
                float height   = drawRect.height / gridSize;

                int nonNull = 0;

                for (int i = 0; i < sprites.Count; i++)
                {
                    Sprite sprite = sprites[i];
                    if (sprite == null) continue;
                    nonNull++;
                    PvCustomizerGUI.DrawSprite(new Rect()
                    {
                        x      = drawRect.x + width  * (i % gridSize),
                        y      = drawRect.y + height * (i / gridSize),
                        width  = width,
                        height = height
                    }, sprite, style.Material, style.Tint);
                }

                if (style.SizeType == IconSizeType.Large)
                {
                    const int size = 10;
                    using (new TempFontSize(size))
                    {
                        string label = $"{nonNull}/{sprites.Count}";
                        GUI.skin.label.CalcMinMaxWidth(new GUIContent(label), out var min, out var max);
                        Rect area = new Rect(drawRect.xMax - min, drawRect.yMax - size / 2f, min * 2, size / 2f);

                        PvCustomizerGUI.DrawTextDirect(area, label, textAnchor: PvAnchor.UpperRight);
                    }
                }
            }
        }

        public bool ValidForType(Type type)
        {
            return typeof(IList<Sprite>).IsAssignableFrom(type);
        }

        public int Priority => PvCustomizerUtility.DefaultPriority;

        #endregion
    }
}