using UnityEngine;

namespace PvCustomizer.Samples
{
    [CreateAssetMenu(menuName = "Pv/Samples/MeshSample", order = 0)]
    public class MeshSample : ScriptableObject
    {
        [PvIcon]
        public Mesh mesh;
    }
}