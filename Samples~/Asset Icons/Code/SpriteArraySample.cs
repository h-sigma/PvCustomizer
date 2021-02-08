using PvCustomizer.Editor;
using UnityEngine;
using FontStyle = PvCustomizer.Editor.FontStyle;

namespace PvCustomizer.Samples
{
    [CreateAssetMenu(menuName = "Pv/Samples/SpriteArraySample", order = 0)]
    public class SpriteArraySample : ScriptableObject
    {
        [PvIcon(fontStyle:FontStyle.Bold, textAnchor: Anchor.MiddleCenter)]
        public Sprite[] sprites;
    }
}