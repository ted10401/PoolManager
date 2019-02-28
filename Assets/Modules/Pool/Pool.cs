using UnityEngine;
using System.Collections.Generic;

namespace JSLCore.Pool
{
    public class Pool<T> where T : class
    {
        private enum PoolType
        {
            None,

            GameObject,
            Component,
            Object,
            Class
        }

        private T m_reference;
        private string m_key;
        private PoolType m_poolType;
        private Transform m_root;
        private Queue<T> m_pool;

        private T m_cacheGeneric;
        private GameObject m_cacheGameObject;
        private Component m_cacheComponent;

        public Pool(string key, T reference, int initialSize)
        {
            m_key = key;
            m_reference = reference;
            m_poolType = GetPoolType(reference);
            m_root = GetRoot(reference);
            m_pool = new Queue<T>();

            Spawn(initialSize);
        }

        private PoolType GetPoolType(T reference)
        {
            if (reference == null)
            {
                return PoolType.None;
            }
            else if (reference is GameObject)
            {
                return PoolType.GameObject;
            }
            else if (reference is Component)
            {
                return PoolType.Component;
            }
            else if (reference is Object)
            {
                return PoolType.Object;
            }
            else
            {
                return PoolType.Class;
            }
        }

        private Transform GetRoot(T reference)
        {
            Transform root = null;

            if (m_poolType == PoolType.GameObject || m_poolType == PoolType.Component)
            {
                root = new GameObject(string.Format("[Pool ({0})] - {1}", typeof(T).Name, m_key)).transform;
                root.SetParent(PoolManager.Instance.transform);
            }

            return root;
        }

        protected void Spawn(int spawnSize)
        {
            if (m_pool.Count >= spawnSize)
            {
                JSLDebug.Log("[Pool] - The pool size is bigger than the spawn size, don't need to create new object.");
                return;
            }

            for (int i = 0; i < spawnSize; i++)
            {
                Create();
            }
        }

        private void Create()
        {
            Recycle(GenerateNewObject());
        }

        private Object m_cacheObject;
        private T GenerateNewObject()
        {
            switch(m_poolType)
            {
                case PoolType.GameObject:
                case PoolType.Component:
                    m_cacheObject = Object.Instantiate(m_reference as Object);
                    m_cacheObject.name = m_key;
                    return m_cacheObject as T;
                case PoolType.Object:
                    m_cacheObject = Object.Instantiate(m_reference as Object);
                    m_cacheObject.name = typeof(T).Name;
                    return m_cacheObject as T;
                default:
                    return null;
            }
        }

        private void CheckingPool()
        {
            if (m_pool.Count <= 0)
            {
                Spawn(PoolManager.DEFAULT_SPAWN_SIZE);
            }
        }

        public virtual T Get()
        {
            CheckingPool();

            m_cacheGeneric = m_pool.Dequeue();

            m_cacheGameObject = null;
            if (m_poolType == PoolType.GameObject)
            {
                m_cacheGameObject = m_cacheGeneric as GameObject;
            }
            else if (m_poolType == PoolType.Component)
            {
                m_cacheComponent = m_cacheGeneric as Component;
                m_cacheGameObject = m_cacheComponent.gameObject;
            }

            if(m_cacheGameObject != null)
            {
                m_cacheGameObject.name = m_key;
                m_cacheGameObject.transform.SetParent(null);
                m_cacheGameObject.SetActive(true);
            }

            return m_cacheGeneric;
        }

        public virtual void Recycle(T asset)
        {
            m_pool.Enqueue(asset);

            m_cacheGameObject = null;
            if(m_poolType == PoolType.GameObject)
            {
                m_cacheGameObject = asset as GameObject;
            }
            else if (m_poolType == PoolType.Component)
            {
                m_cacheComponent = asset as Component;
                m_cacheGameObject = m_cacheComponent.gameObject;
            }

            if(m_cacheGameObject != null)
            {
                m_cacheGameObject.transform.SetParent(m_root);
                m_cacheGameObject.SetActive(false);
            }
        }

        public void Clear()
        {
            while (m_pool.Count != 0)
            {
                if(m_reference is Object)
                {
                    Object.Destroy(m_pool.Dequeue() as Object);
                }
                else
                {
                    m_pool.Dequeue();
                }
            }

            m_pool.Clear();
        }

        public void Destroy()
        {
            Clear();

            m_reference = null;
            if (m_root != null)
            {
                GameObject.Destroy(m_root.gameObject);
            }
        }
    }
}