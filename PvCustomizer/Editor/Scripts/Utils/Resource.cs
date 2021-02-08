using System.IO;
using UnityEditor;

namespace Akaal.Editor.Utils
{
    public static class Resource
    {
        private const string resourcePath =
            "Packages/" + PvCustomizerSettings.PackageName + "/PvCustomizer/Editor/Resources";

        public static T Load<T>(string path) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(Path.Combine(resourcePath, path));
        }
    }
}