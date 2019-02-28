using UnityEngine;
using System.Collections.Generic;

namespace JSLCore.Pool
{
    public class Pool<T> where T : class
    {
        private T m_reference;
        private string m_key;
        private PoolType m_poolType;
        private Transform m_root;
        private Queue<T> m_pool;

        private T m_cacheGeneric;
        private GameObject m_cacheGameObject;
        private Component m_cacheComponent;

        public Pool(T reference, int initialSize)
        {
            m_reference = reference;
            m_key = m_reference.GetKey();
            m_poolType = m_reference.GetPoolType();
            m_root = m_reference.GetRoot();
            m_pool = new Queue<T>();

            Spawn(initialSize);
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
                case PoolType.Object:
                    m_cacheObject = Object.Instantiate(m_reference as Object);
                    m_cacheObject.name = m_key;
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