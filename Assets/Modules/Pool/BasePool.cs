using UnityEngine;
using System.Collections.Generic;

namespace JSLCore.Pool
{
    public class BasePool<T> : IDestroy where T : Object
    {
        protected GameObject m_root;
        private string m_typeName;
        protected string m_key;
        private T m_referenceAsset;
        protected Queue<T> m_pool;

        protected BasePool(string key, T referenceAsset, int initialSize)
        {
            m_key = key;

            if (string.IsNullOrEmpty(m_key))
            {
                JSLDebug.LogError("[ObjectPool] - The key is null or empty.");
                return;
            }

            if (referenceAsset == null)
            {
                JSLDebug.LogError("[ObjectPool] - The reference asset is null.");
                return;
            }

            m_root = new GameObject(string.Format("[ObjectPool ({0})] - {1}", typeof(T).Name, m_key));
            m_root.transform.SetParent(PoolManager.Instance.transform, false);
            m_referenceAsset = referenceAsset;
            m_pool = new Queue<T>();

            Spawn(initialSize);
        }

        protected void Spawn(int spawnSize)
        {
            if(m_pool.Count >= spawnSize)
            {
                JSLDebug.Log("[ObjectPool] - The pool size is bigger than the spawn size, don't need to create new object.");
                return;
            }

            for (int i = 0; i < spawnSize; i++)
            {
                Create();
            }
        }
        
        private void Create()
        {
            Recycle(Object.Instantiate(m_referenceAsset));
        }

        private void CheckingPool()
        {
            if (m_pool.Count <= 0)
            {
                Spawn(PoolManager.Instance.DEFAULT_SPAWN_SIZE);
            }
        }

        public virtual T Get()
        {
            CheckingPool();
            return m_pool.Dequeue();
        }

        public virtual void Recycle(T asset)
        {
            m_pool.Enqueue(asset);
        }

        public void Destroy()
        {
            Object.Destroy(m_root);
        }
    }
}