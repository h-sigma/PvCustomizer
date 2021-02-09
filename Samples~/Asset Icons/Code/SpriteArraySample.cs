using Akaal;
using UnityEngine;

namespace PvCustomizer.Editor.Samples
{
    public class SpriteArraySample : ScriptableObject
    {
        [PvIcon(fontStyle:PvFontStyle.Bold, textAnchor: PvAnchor.MiddleCenter, display: "large")]
        public Sprite[] sprites;

        [PvIcon(layer: 1,  display: "small")]
        public Sprite sprite;
    }
}