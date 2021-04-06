namespace Akaal.PvCustomizer.Scripts
{
    public enum IconSizeType
    {
        Small,
        Large,
        TreeView
    }

    public enum PvAnchor
    {
        /// <summary>
        ///   <para>Text is anchored in upper left corner.</para>
        /// </summary>
        UpperLeft,

        /// <summary>
        ///   <para>Text is anchored in upper side, centered horizontally.</para>
        /// </summary>
        UpperCenter,

        /// <summary>
        ///   <para>Text is anchored in upper right corner.</para>
        /// </summary>
        UpperRight,

        /// <summary>
        ///   <para>Text is anchored in left side, centered vertically.</para>
        /// </summary>
        MiddleLeft,

        /// <summary>
        ///   <para>Text is centered both horizontally and vertically.</para>
        /// </summary>
        MiddleCenter,

        /// <summary>
        ///   <para>Text is anchored in right side, centered vertically.</para>
        /// </summary>
        MiddleRight,

        /// <summary>
        ///   <para>Text is anchored in lower left corner.</para>
        /// </summary>
        LowerLeft,

        /// <summary>
        ///   <para>Text is anchored in lower side, centered horizontally.</para>
        /// </summary>
        LowerCenter,

        /// <summary>
        ///   <para>Text is anchored in lower right corner.</para>
        /// </summary>
        LowerRight,
    }

    /// <summary>
    /// Alias-like enum for Unity's FontStyle.
    /// </summary>
    public enum PvFontStyle
    {
        /// <summary>
        ///   <para>No special style is applied.</para>
        /// </summary>
        Normal,

        /// <summary>
        ///   <para>Bold style applied to your texts.</para>
        /// </summary>
        Bold,

        /// <summary>
        ///   <para>Italic style applied to your texts.</para>
        /// </summary>
        Italic,

        /// <summary>
        ///   <para>Bold and Italic styles applied to your texts.</para>
        /// </summary>
        BoldAndItalic,
    }

    /// <summary>
    /// Alias-like enum for Unity's ScaleMode.
    /// </summary>
    public enum PvScaleMode
    {
        /// <summary>
        ///   <para>Stretches the texture to fill the complete rectangle passed in to GUI.DrawTexture.</para>
        /// </summary>
        StretchToFill,

        /// <summary>
        ///   <para>Scales the texture, maintaining aspect ratio, so it completely covers the position rectangle passed to GUI.DrawTexture. If the texture is being draw to a rectangle with a different aspect ratio than the original, the image is cropped.</para>
        /// </summary>
        ScaleAndCrop,

        /// <summary>
        ///   <para>Scales the texture, maintaining aspect ratio, so it completely fits withing the position rectangle passed to GUI.DrawTexture.</para>
        /// </summary>
        ScaleToFit,
    }
}