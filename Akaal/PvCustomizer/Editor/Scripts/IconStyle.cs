using Akaal.PvCustomizer.Scripts;
using UnityEngine;

namespace Akaal.PvCustomizer.Editor
{
    /// <summary>
    /// A set of style properties to guide drawing of the icon, compiled from the ProjectIconAttribute values.
    /// </summary>
    public struct IconStyle
    {
        /// <summary>
        /// Default-check for struct.
        /// </summary>
        public bool IsSet;

        /// <summary>
        /// The Rect according to attribute data that should hold all of the icon.
        /// </summary>
        public Rect DrawRect;

        /// <summary>
        /// A list of custom values passed in from the attribute. Use this for your custom values.
        /// </summary>
        public object[] CustomValues;

        /// <summary>
        /// The Font Style used to render all of the text.
        /// </summary>
        public PvFontStyle FontStyle;

        /// <summary>
        /// A Material to be used to draw the icon. The default material passed in is a cached "Sprites/Default".
        /// </summary>
        public Material Material;

        /// <summary>
        /// A tint to be applied.
        /// </summary>
        public Color Tint;

        /// <summary>
        /// Size type of the anchor depending on project view magnification.
        /// </summary>
        public IconSizeType SizeType;

        /// <summary>
        /// A maximum size for the icon to be drawn. Negative value means fit to full icon rectangle.
        /// </summary>
        public int MaxSize;

        /// <summary>
        /// An anchor for all of the text rendered.
        /// </summary>
        public PvAnchor TextAnchor;

        /// <summary>
        /// How the graphic is to be scaled to fit its area.
        /// </summary>
        public PvScaleMode ScaleMode;
    }
}