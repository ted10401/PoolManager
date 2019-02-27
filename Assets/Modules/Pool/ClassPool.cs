using UnityEngine;
using System.Collections.Generic;

namespace JSLCore.Pool
{
    public class ClassPool<T> where T : class
    {
        private const int DEFAULT_SPAWN_SIZE = 5;

        private enum PoolType
        {
            None,

            GameObject,
            Component,
            Object,
            Class
        }

        private string m_key;
        private T m_reference;
        private PoolType m_poolType;
        private Transform m_root;
        protected Queue<T> m_pool;

        public ClassPool(T reference, int initialSize)
        {
            m_reference = reference;
            UpdatePoolType();

            m_pool = new Queue<T>();
            Spawn(initialSize);
        }

        private void UpdatePoolType()
        {
            if (m_reference == null)
            {
                m_poolType = PoolType.None;
            }
            else if (m_reference is GameObject)
            {
                m_poolType = PoolType.GameObject;
                m_key = (m_reference as GameObject).name;
                m_root = new GameObject(string.Format("[Pool ({0})] - {1}", typeof(T).Name, m_key)).transform;
                m_root.SetParent(PoolManager.Instance.transform);
            }
            else if(m_reference is Component)
            {
                m_poolType = PoolType.Component;
                m_key = (m_reference as Component).gameObject.name;
                m_root = new GameObject(string.Format("[Pool ({0})] - {1}", typeof(T).Name, m_key)).transform;
                m_root.SetParent(PoolManager.Instance.transform);
            }
            else if(m_reference is Object)
            {
                m_poolType = PoolType.Object;
                m_key = (m_reference as Object).name;
            }
            else
            {
                m_poolType = PoolType.Class;
            }
        }

        protected void Spawn(int spawnSize)
        {
            if (m_pool.Count >= spawnSize)
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
            Recycle(GenerateNewObject());
        }

        protected virtual T GenerateNewObject()
        {
            switch(m_poolType)
            {
                case PoolType.GameObject:
                case PoolType.Component:
                case PoolType.Object:
                    Object cache = Object.Instantiate(m_reference as Object);
                    cache.name = m_key;
                    return cache as T;
                default:
                    return null;
            }
        }

        private void CheckingPool()
        {
            if (m_pool.Count <= 0)
            {
                Spawn(DEFAULT_SPAWN_SIZE);
            }
        }

        public virtual T Get()
        {
            CheckingPool();

            T cache = m_pool.Dequeue();
            if(m_poolType == PoolType.GameObject)
            {
                GameObject gameObject = cache as GameObject;
                gameObject.name = m_key;
                gameObject.transform.SetParent(null);
                gameObject.SetActive(true);
            }
            else if (m_poolType == PoolType.Component)
            {
                Component component = cache as Component;
                component.gameObject.name = m_key;
                component.transform.SetParent(null);
                component.gameObject.SetActive(true);
            }

            return cache;
        }

        public virtual void Recycle(T asset)
        {
            m_pool.Enqueue(asset);

            if(m_poolType == PoolType.GameObject)
            {
                GameObject cache = asset as GameObject;
                cache.transform.SetParent(m_root);
                cache.SetActive(false);
            }
            else if (m_poolType == PoolType.Component)
            {
                Component cache = asset as Component;
                cache.transform.SetParent(m_root);
                cache.gameObject.SetActive(false);
            }
        }

        public void Clear()
        {
            while (m_pool.Count != 0)
            {
                if(m_poolType != PoolType.None && m_poolType != PoolType.Class)
                {
                    Object.Destroy(m_pool.Dequeue() as Object);
                }
            }

            m_pool.Clear();
        }
    }
}