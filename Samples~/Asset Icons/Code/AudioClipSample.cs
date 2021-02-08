using UnityEngine;

namespace PvCustomizer.Samples
{
    [CreateAssetMenu(menuName = "Pv/Samples/AudioClipSample", order = 0)]
    public class AudioClipSample : ScriptableObject
    {
        [PvIcon]
        public AudioClip clip;
    }
}