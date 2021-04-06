using System;
using UnityEngine;

namespace Akaal.PvCustomizer.Scripts
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Property |
                    AttributeTargets.Method)]
    public class PvIconAttribute : PropertyAttribute
    {
        public bool        DontEraseDefault;
        public string      Width;
        public string      Height;
        public string      X;
        public string      Y;
        public PvScaleMode ScaleMode;
        public Color       Tint;
        public int         MaxSize;
        public PvAnchor      IconAnchor;
        public string      Display;
        public int         Layer;
        public PvFontStyle   FontStyle;
        public string      Material;
        public string      Grid;
        public object[]    CustomData;
        public PvAnchor      TextAnchor;

        /// <summary>
        /// Creates an attribute of the given type.
        /// </summary>
        /// <param name="width">Width of the icon. Append percentage symbol for relative to max width.</param>
        /// <param name="height">Height of the icon. Append percentage symbol for relative to max height.</param>
        /// <param name="x">Horizontal position of icon from top-left corner. Append percentage for relative to width.</param>
        /// <param name="y">Vertical position of icon from top-left corner. Append percentage for relative to height.</param>
        /// <param name="tint">The color to tint the final icon graphic with.</param>
        /// <param name="maxSize"></param>
        /// <param name="iconAnchor"></param>
        /// <param name="textAnchor"></param>
        /// <param name="display"></param>
        /// <param name="layer"></param>
        /// <param name="fontStyle"></param>
        /// <param name="dontEraseDefault"></param>
        /// <param name="grid"></param>
        /// <param name="customValues">Array of custom values to be passed to your custom drawers..</param>
        public PvIconAttribute(string width = "100%", string height = "100%", string x = "0", string y = "0",
            PvScaleMode scaleMode = PvScaleMode.ScaleToFit,
            string tint = "#FFFFFFFF", int maxSize = 128,
            PvAnchor iconAnchor = PvAnchor.MiddleCenter, PvAnchor textAnchor = PvAnchor.MiddleCenter,
            string display = "", int layer = 0, PvFontStyle fontStyle = PvFontStyle.Normal, string material = "",
            bool dontEraseDefault = false, string grid = "",
            object[] customValues = null)
        {
            Width     = width;
            Height    = height;
            X         = x;
            Y         = y;
            ScaleMode = scaleMode;
            if (!ColorUtility.TryParseHtmlString(tint, out Tint)) Tint = Color.white;
            MaxSize          = maxSize;
            IconAnchor       = iconAnchor;
            Display          = display;
            Layer            = layer;
            FontStyle        = fontStyle;
            Material         = material;
            DontEraseDefault = dontEraseDefault;
            TextAnchor       = textAnchor;
            Grid             = grid;
            CustomData       = customValues ?? Array.Empty<object>();
        }
    }
}