using System.Collections.Generic;
using UnityEngine;

namespace Akaal.Editor.Utils
{
    public static class MaterialCopyCache
    {
        private static Dictionary<Material, Material> _materials = new Dictionary<Material, Material>();

        public static Material GetCached(Material material)
        {
            if (_materials.TryGetValue(material, out var copy))
            {
                copy.CopyPropertiesFromMaterial(material); //update
                return copy;
            }

            copy                 = new Material(material);
            _materials[material] = copy;
            return copy;
        }
    }
}