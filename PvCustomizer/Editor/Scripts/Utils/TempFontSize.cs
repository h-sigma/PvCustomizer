using System;
using UnityEngine;

namespace Akaal.Editor.Utils
{
    public struct TempFontSize : IDisposable
    {
        private TempValueArea<int> _temp;

        public TempFontSize(int newSize)
        {
            _temp = new TempValueArea<int>(newSize, Getter, Setter);
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _temp.Dispose();
        }

        #endregion

        private static void Setter(int v)
        {
            GUI.skin.label.fontSize = v;
        }

        private static int Getter()
        {
            return GUI.skin.label.fontSize;
        }
    }
}