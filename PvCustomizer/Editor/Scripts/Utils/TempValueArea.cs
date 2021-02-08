using System;

namespace Akaal.Editor.Utils
{
    public struct TempValueArea<T> : IDisposable
    {
        private T         _tempValue;
        private Action<T> _setter;

        public TempValueArea(T newValue, Func<T> getter, Action<T> setter)
        {
            _tempValue = getter.Invoke();
            _setter    = setter;
            setter.Invoke(newValue);
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _setter.Invoke(_tempValue);
        }

        #endregion
    }
}