using Akaal;
using UnityEngine;

namespace PvCustomizer.Editor.Samples
{
    public class SpriteArraySample : ScriptableObject
    {
        [PvIcon(fontStyle:PvFontStyle.Bold, textAnchor: PvAnchor.MiddleCenter)]
        public Sprite[] sprites;
    }
}