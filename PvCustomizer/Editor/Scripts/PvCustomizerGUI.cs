using Akaal.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Akaal.Editor
{
    public static class PvCustomizerGUI
    {
        #region Privates

        private static readonly Color32 BG_COLOR_FREE = new Color32(190, 190, 190, 255);
        private static readonly Color32 BG_COLOR_PRO  = new Color32(51,  51,  51,  255);

        //public static readonly Color32 ICON_SELECTED_BLUE = new Color32(44, 93, 135, 255);
        public static readonly Color32 ICON_SELECTED_TINT = new Color32(200, 200, 255, 255);

        public static readonly Color32 ICON_SELECTED_BLUE_BACKGROUND = new Color32(61, 128, 223, 255);

        private static void DrawTextureDirect(Rect rect, Texture2D texture, Material material, PvScaleMode? scaleMode,
            Color? tint = null)
        {
            if (texture == null) return;
            material.SetFloat(s_TintAmount, PvCustomizerSettings.GetOrCreateSettings().TintAmount);
            material.SetColor(s_Tint, tint ?? Color.white);
            UnityEditor.EditorGUI.DrawPreviewTexture(rect, texture, material,
                (scaleMode ?? PvScaleMode.ScaleToFit).UnityScaleMode());
        }

        #endregion

        /// <summary>
        /// A color that imitates the background color of the Unity window, used to hide default drawer.
        /// </summary>
        public static Color UnityBackground => EditorGUIUtility.isProSkin ? BG_COLOR_PRO : BG_COLOR_FREE;

        public static Rect WindowRect { get; set; }

        public static bool InvertedColors
        {
            get => GetMaterialForUse(null).GetInt(s_Inverted) == 1;
            set => GetMaterialForUse(null).SetInt(s_Inverted, value ? 1 : 0);
        }

        #region Public GUI Methods

        /// <summary>
        /// Draws Unity's Background color, often used to mask the default project icon.
        /// </summary>
        /// <param name="rect">Icon rect to draw over.</param>
        public static void DrawBackground(Rect rect)
        {
            UnityEditor.EditorGUI.DrawRect(rect, UnityBackground);
        }

        /// <summary>
        /// Draw a texture in the given rectangle that scales and aspects according to the icon style.
        /// </summary>
        /// <param name="rect">Rectangle to draw the texture inside.</param>
        /// <param name="texture">Texture to draw.</param>
        /// <param name="style">The compiled style for the icon.</param>
        /// <param name="material">Custom material used to draw the texture. 'Sprites/Default' by default.</param>
        public static void DrawTexture(Rect rect, Texture2D texture, Material material = null, Color? tint = null,
            PvScaleMode? scaleMode = null)
        {
            if (texture == null) return;
            material = GetMaterialForUse(material);
            ResetTiling(material);
            DrawTextureDirect(rect, texture, material, tint: tint, scaleMode: scaleMode);
        }

        /// <summary>
        /// Draws a flat, optionally transparent, color at the specified rect.
        /// </summary>
        /// <param name="rect">The rect that defines the color bounds.</param>
        /// <param name="color">The color that is rendered. May be transparent.</param>
        /// <param name="style">The compiled style for the icon.</param>
        /// <param name="applyTint">Whether to apply a tint. May be synonymous to whether icon's item is selected.</param>
        public static void DrawColor(Rect rect, Color color)
        {
            UnityEditor.EditorGUI.DrawRect(rect, color);
        }

        /// <summary>
        /// Draws a sprite at the specified rect, fully respecting its offset. Do not pass in a sprite that is in a tightly packed atlas.
        /// </summary>
        /// <param name="rect">The rect that specifies drawing bounds.</param>
        /// <param name="sprite">The sprite to be drawn.</param>
        /// <param name="style">The compiled style for the icon.</param>
        /// <param name="material">The Material used to draw the sprite. Leave null to use default.</param>
        public static void DrawSprite(Rect rect, Sprite sprite, Material material = null, Color? tint = null,
            PvScaleMode? scaleMode = null)
        {
            if (sprite == null) return;
            Rect texRect = sprite.textureRect;

            if (scaleMode == PvScaleMode.ScaleToFit)
            {
                float drawAspect = rect.Aspect();
                float texAspect  = texRect.Aspect();
                if (drawAspect > texAspect) //drawer rect is wider than tex rect
                {
                    float widthToAdd = texRect.height * drawAspect;
                    texRect.x     -= (widthToAdd - texRect.width) / 2f;
                    texRect.width =  widthToAdd;
                }
                else
                {
                    float heightToAdd = texRect.width / drawAspect;
                    texRect.y      -= (heightToAdd - texRect.height) / 2f;
                    texRect.height =  heightToAdd;
                }

            }

            DrawTexWithCoords(rect, sprite.texture, texRect, material, tint: tint, scaleMode: scaleMode);
        }

        /// <summary>
        /// Draw a texture in the given rectangle that scales and aspects according to the icon style and textureCoords.
        /// </summary>
        /// <param name="rect">Rectangle to draw the texture inside.</param>
        /// <param name="texture">Texture to draw.</param>
        /// <param name="textureCoords">The pixel rect of the texture to be drawn.</param>
        /// <param name="style">The compiled style for the icon.</param>
        /// <param name="material">Custom material used to draw the texture. 'Sprites/Default' by default.</param>
        public static void DrawTexWithCoords(Rect rect, Texture2D texture, Rect textureCoords,
            Material material = null, Color? tint = null, PvScaleMode? scaleMode = null)
        {
            material = GetMaterialForUse(material);
            SetTiling(material, textureCoords, texture.Size());
            DrawTextureDirect(rect, texture, material, scaleMode: scaleMode, tint: tint);
            ResetTiling(material);
        }


        /// <summary>
        /// Draw a texture in the given rectangle that scales and aspects according to the icon style.
        /// </summary>
        /// <param name="rect">Rectangle to draw the texture inside.</param>
        /// <param name="texture">Texture to draw.</param>
        /// <param name="uvCoords">The UV rect of the texture to be drawn.</param>
        /// <param name="style">The compiled style for the icon.</param>
        /// <param name="material">Custom material used to draw the texture. 'Sprites/Default' by default.</param>
        public static void DrawTexWithUVCoords(Rect rect, Texture2D texture, Rect uvCoords,
            Material material = null, Color? tint = null, PvScaleMode? scaleMode = null)
        {
            material = GetMaterialForUse(material);
            SetTiling(material, uvCoords);
            DrawTextureDirect(rect, texture, material, tint: tint, scaleMode: scaleMode);
            ResetTiling(material);
        }

        private static GUIContent content = new GUIContent();

        public static void DrawTextDirect(Rect rect, string text, Color? color = null,
            UnityEngine.FontStyle fontStyle = UnityEngine.FontStyle.Normal,
            PvAnchor textAnchor = PvAnchor.MiddleLeft)
        {
            var tempCol       = GUI.skin.label.normal.textColor;
            var tempStyle     = GUI.skin.label.fontStyle;
            var tempAlignment = GUI.skin.label.alignment;

            GUI.skin.label.normal.textColor = color ?? tempCol;
            GUI.skin.label.fontStyle        = fontStyle.UnityFontStyle();
            GUI.skin.label.alignment        = textAnchor.UnityTextAnchor();

            GUI.Label(rect, text);

            GUI.skin.label.alignment        = tempAlignment;
            GUI.skin.label.normal.textColor = tempCol;
            GUI.skin.label.fontStyle        = tempStyle;
        }

        public static void DrawBorder(Rect rect, Color color, float width = 1f)
        {
            Rect line = new Rect(rect.x - width, rect.y - width, rect.width + width * 2, width);
            UnityEditor.EditorGUI.DrawRect(line, color); //top
            line.y = rect.y + rect.height - 1f;
            UnityEditor.EditorGUI.DrawRect(line, color); //bottom
            line.width  = width;
            line.height = rect.height;
            line.y      = rect.y;
            UnityEditor.EditorGUI.DrawRect(line, color); //left
            line.x = rect.x + rect.width;
            UnityEditor.EditorGUI.DrawRect(line, color); //right
        }

        /// <summary>
        /// Draws text within the given Rect, wrapping across horizontal, and cut-off along bottom.
        /// Uses font size as in GUI.skin.label.fontSize.
        /// </summary>
        /// <param name="rect">The rectangle to draw the text within.</param>
        /// <param name="text">The text to be drawn.</param>
        /// <param name="color">Optional color to draw the text in.</param>
        /// <param name="useColor">Whether to use the color.</param>
        public static void DrawText(Rect rect, string text, out Rect occupiedText,
            Color? color = default, UnityEngine.FontStyle fontStyle = UnityEngine.FontStyle.Normal)
        {
            var tempCol   = GUI.skin.label.normal.textColor;
            var tempStyle = GUI.skin.label.fontStyle;

            GUI.skin.label.normal.textColor = color ?? GUI.skin.label.normal.textColor;
            GUI.skin.label.fontStyle        = fontStyle.UnityFontStyle();

            content.text = text;
            GUI.skin.label.CalcMinMaxWidth(content, out float fullMin, out float fullMax);
            float height = GUI.skin.label.CalcHeight(content, fullMin);

            int   row   = 0;
            float start = 0;

            Rect toDraw = new Rect(rect) {height = GUI.skin.label.fontSize + 1};
            int  length = Mathf.FloorToInt(rect.width * text.Length / fullMin);
            while (height * row < rect.height)
            {
                int startIndex                           = Mathf.FloorToInt(start * text.Length / fullMin);
                if (startIndex > text.Length) startIndex = text.Length;
                content.text = text.Substring(startIndex, Mathf.Min(length, text.Length - startIndex));
                toDraw.y     = row * height + rect.y;
                GUI.Label(toDraw, content);
                if (start > fullMin) break;
                row++;
                start += rect.width;
            }

            occupiedText = new Rect(rect.x, rect.y, rect.width, Mathf.Min(row * height, rect.height));

            GUI.skin.label.normal.textColor = tempCol;
            GUI.skin.label.fontStyle        = tempStyle;
        }

        #endregion

        #region Material Helper

        private static Material DefaultMaterial;

        public static readonly Material AssetPreviewClipMaterial =
            new Material(Shader.Find("Hidden/PvCustomizer/DefaultIcon"));

        private static Material GetMaterialForUse(Material material)
        {
            if (material == null)
            {
                if (DefaultMaterial == null)
                    DefaultMaterial = new Material(Shader.Find("Hidden/PvCustomizer/DefaultIcon"));
                return DefaultMaterial;
            }

            return MaterialCopyCache.GetCached(material);
        }

        private static readonly int s_MainTexProperty       = Shader.PropertyToID("_MainTex");
        private static readonly int s_MainTexTilingProperty = Shader.PropertyToID("_MainTex_ScaleTiling");
        private static readonly int s_Tint                  = Shader.PropertyToID("_Tint");
        private static readonly int s_TintAmount            = Shader.PropertyToID("_TintAmount");
        private static readonly int s_Inverted              = Shader.PropertyToID("_Invert");

        private static void SetTiling(Material material, Rect texCoords, Vector2 texSize)
        {
            Vector2 inverseTexSize = new Vector2(1 / texSize.x, 1 / texSize.y);
            Vector2 uvScale        = texCoords.size * inverseTexSize;
            SetTiling(material,
                new Rect(texCoords.position * inverseTexSize, new Vector2(uvScale.x, uvScale.y)));
        }

        private static void ResetTiling(Material material)
        {
            SetTiling(material, new Rect(Vector2.zero, Vector2.one));
        }

        private static void SetTiling(Material material, Rect uvCoords)
        {
            material.SetVector(s_MainTexTilingProperty,
                new Vector4(uvCoords.width, uvCoords.height, uvCoords.x, uvCoords.y));
        }

        #endregion
    }
}