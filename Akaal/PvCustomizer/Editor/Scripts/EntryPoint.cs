using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;

namespace Akaal.PvCustomizer.Editor
{
    public static class EntryPoint
    {
        private static PvCustomizerMain _main;

        [InitializeOnLoadMethod]
        private static void RegisterToProjectWindow()
        {
            //required for regex matches;
            //we will also use compiled regex matches
            Regex.CacheSize = 200;

            //attach drawers to hook
            _main                                    =  new PvCustomizerMain();
            EditorApplication.projectWindowItemOnGUI += _main.DrawProjectIcon;
            foreach (Type type in TypeCache.GetTypesDerivedFrom<IDrawer>().Where(t => !t.IsAbstract))
            {
                _main.Registry.RegisterIconDrawer(Activator.CreateInstance(type) as IDrawer);
            }
            
        }
    }
}