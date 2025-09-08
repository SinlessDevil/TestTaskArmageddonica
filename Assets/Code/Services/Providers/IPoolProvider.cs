using System.Collections.ObjectModel;
using UnityEngine;

namespace Code.Services.Providers
{
    public interface IPoolProvider<T> where T : Component
    {
        public void CreatePool(int initialCount, Transform parent = null);
        public void CleanupPool();
        public T Get(Vector3 position, Quaternion rotation, Transform parent = null);
        public void Return(T item);
        void CreatePool();
        ReadOnlyCollection<T> GetPoolSnapshot();
    }   
}