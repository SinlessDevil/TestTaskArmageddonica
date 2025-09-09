using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Code.Services.Providers
{
    public abstract class BasePoolProvider<T> : IPoolProvider<T> where T : Component
    {
        protected readonly IPoolFactory<T> _factory;
        protected readonly List<T> _pool = new();

        protected Transform _defaultParent;

        protected BasePoolProvider(IPoolFactory<T> factory)
        {
            _factory = factory;
        }

        public abstract void CreatePool();
        
        public virtual void CreatePool(int initialCount, Transform parent = null)
        {
            _defaultParent = parent;

            for (int i = 0; i < initialCount; i++)
            {
                T item = CreateObject(Vector3.zero, Quaternion.identity, _defaultParent);
                Deactivate(item);
            }
        }

        public virtual void CleanupPool()
        {
            foreach (var item in _pool.Where(i => i != null))
                Object.Destroy(item.gameObject);
            
            _pool.Clear();
        }

        public virtual T Get(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            foreach (var item in _pool.Where(item => item != null && IsAvailable(item)))
            {
                PrepareTransform(item.transform, position, rotation, parent ?? _defaultParent);
                Activate(item);
                return item;
            }
            
            T created = CreateObject(position, rotation, parent ?? _defaultParent);
            Activate(created);
            return created;
        }

        public virtual void Return(T item)
        {
            if (item == null) 
                return;
            
            Deactivate(item);
            
            if (_defaultParent) 
                item.transform.SetParent(_defaultParent, false);
        }
        
        public ReadOnlyCollection<T> GetPoolSnapshot()
            => _pool.Where(i => i != null).ToList().AsReadOnly();
        
        protected virtual bool IsAvailable(T item)
        {
            return !item.gameObject.activeInHierarchy;
        }

        protected virtual void Activate(T item)
        {
            item.gameObject.SetActive(true);
        }

        protected virtual void Deactivate(T item)
        {
            item.gameObject.SetActive(false);
        }
        
        protected virtual T CreateObject(Vector3 pos, Quaternion rot, Transform parent)
        {
            var created = _factory.Create(pos, rot, parent);
            _pool.Add(created);
            return created;
        }

        protected static void PrepareTransform(Transform transform, Vector3 pos, Quaternion rot, Transform parent)
        {
            if (parent) 
                transform.SetParent(parent, worldPositionStays: false);
            transform.SetPositionAndRotation(pos, rot);
        }
    }
}
