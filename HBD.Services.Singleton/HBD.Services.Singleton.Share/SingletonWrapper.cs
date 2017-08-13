#region using

using System;

#endregion

namespace HBD.Services.Singleton
{
    internal interface ISingletonWrapper : IDisposable
    {
        object Instance { get; }
        void Reset();
    }

    /// <summary>
    ///     Support to load instance of class 1 time only.
    /// </summary>
    public sealed class SingletonWrapper<T> : ISingletonWrapper
    {
        private readonly Func<T> _factoryFunc;
        private T _instance;
        private bool _isDisposed;
        private bool _isLoaded;

        public SingletonWrapper(Func<T> factoryFunc)
        {
            _factoryFunc = factoryFunc;
        }

        public T Instance
        {
            get
            {
                ValidateDiposed();
                if (_isLoaded) return _instance;
               
                //Try to disposed the old object.
                Dispose(false);
                //Load new instance.
                _instance = _factoryFunc.Invoke();
                //Mark is loaded.
                _isLoaded = true;
                //Return the new instance.
                return _instance;
            }
        }

        private void ValidateDiposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException($"SingletonWrapper of {typeof(T).FullName}",
                    $"SingletonWrapper of {typeof(T).FullName} is Disposed.");
        }

        object ISingletonWrapper.Instance => Instance;

        /// <summary>
        ///     Reset and load instance again on next accessing.
        /// </summary>
        public void Reset() => _isLoaded = false;

        public void Dispose()
        {
            if (_instance == null || _isDisposed) return;
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            var dis = _instance as IDisposable;
            dis?.Dispose();
            _isDisposed = isDisposing;
        }
    }
}