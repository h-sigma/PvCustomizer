using System;

namespace Akaal.PvCustomizer.Editor.EditorGUI
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FetchAttribute : System.Attribute
    {
        public string className;
        public string name;
        public string bindingPath;

        public FetchAttribute(string name = null, string className = null, string bindingPath = null)
        {
            this.className   = className;
            this.name        = name;
            this.bindingPath = bindingPath;
        }
    }
}