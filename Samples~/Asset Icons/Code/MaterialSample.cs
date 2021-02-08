using UnityEngine;

namespace PvCustomizer.Samples
{
    [CreateAssetMenu(menuName = "Pv/Samples/MaterialSample", order = 0)]
    public class MaterialSample : ScriptableObject
    {
        [PvIcon]
        public Material material;
    }
}