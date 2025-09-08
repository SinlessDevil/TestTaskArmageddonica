using UnityEngine;

namespace Code.Services.Providers
{
    public interface IPoolFactory<T> where T : Component
    {
        public T Create(Vector3 position, Quaternion rotation, Transform parent = null);
    }
}