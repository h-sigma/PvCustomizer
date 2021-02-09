using Akaal;
using UnityEngine;

namespace PvCustomizer.Editor.Samples
{
    public class GridSample : ScriptableObject
    {
        [PvIcon(material: nameof(material1), grid:"2/2:0")]
        public Texture2D texture1;

        public Material material1;
        
        [PvIcon(material: nameof(material2), grid:"2/2:1")]
        public Texture2D texture2;

        public Material material2;
        
        [PvIcon(material: nameof(material3), grid:"2/2:2")]
        public Texture2D texture3;

        public Material material3;
        
        [PvIcon(material: nameof(material4), grid:"2/2:3")]
        public Texture2D texture4;

        public Material material4;
    }
}