using UnityEngine;
using System.Collections.Generic;

namespace JSLCore.Pool
{
    public class PoolManager : MonoSingleton<PoolManager>
	{
        public const int DEFAULT_SPAWN_SIZE = 5;

        public void AddPool<T>(T reference, int initialSize) where T : class
        {
            PoolCollections<T>.AddPool(reference, initialSize);
        }

        public T Get<T>(T reference) where T : class
        {
            return PoolCollections<T>.GetPool(reference).Get();
        }

        public void Recycle<T>(T reference) where T : class
        {
            PoolCollections<T>.GetPool(reference).Recycle(reference);
        }

        public void Clear<T>(T reference) where T : class
        {
            PoolCollections<T>.ClearPool(reference);
        }

        public void Destroy<T>(T reference) where T : class
        {
            PoolCollections<T>.DestroyPool(reference);
        }

        private static class PoolCollections<T> where T : class
        {
            private static Pool<T> m_defaultPool;
            private static Dictionary<string, Pool<T>> m_pools;

            public static void AddPool(T reference, int initialSize)
            {
                string key = GetKey(reference);

                if (string.IsNullOrEmpty(key))
                {
                    InitializeDefaultPool(reference, initialSize);
                }
                else
                {
                    InitializePools(key, reference, initialSize);
                }
            }

            public static Pool<T> GetPool(T reference)
            {
                string key = GetKey(reference);

                if (string.IsNullOrEmpty(key))
                {
                    InitializeDefaultPool(reference, DEFAULT_SPAWN_SIZE);
                    return m_defaultPool;
                }
                else
                {
                    InitializePools(key, reference, DEFAULT_SPAWN_SIZE);
                    return m_pools[key];
                }
            }

            public static void ClearPool(T reference)
            {
                GetPool(reference).Clear();
            }

            public static void DestroyPool(T reference)
            {
                string key = GetKey(reference);

                if (string.IsNullOrEmpty(key))
                {
                    if(m_defaultPool != null)
                    {
                        m_defaultPool.Destroy();
                        m_defaultPool = null;
                    }
                }
                else
                {
                    if(m_pools.ContainsKey(key))
                    {
                        m_pools[key].Destroy();
                        m_pools.Remove(key);
                    }
                }
            }

            private static string GetKey(T reference)
            {
                string key = string.Empty;

                if (reference is GameObject)
                {
                    key = (reference as GameObject).name;
                }
                else if (reference is Component)
                {
                    key = (reference as Component).gameObject.name;
                }
                else if (reference is Object)
                {
                    key = (reference as Object).name;
                }

                return key;
            }

            private static void InitializeDefaultPool(T reference, int initialSize)
            {
                if (m_defaultPool == null)
                {
                    m_defaultPool = new Pool<T>(reference, initialSize);
                    JSLDebug.LogFormat("[PoolCollections] - There is no default pool for type '{0}', create a new one.", typeof(T).Name);
                }
            }

            private static void InitializePools(string key, T reference, int initialSize)
            {
                if (m_pools == null)
                {
                    m_pools = new Dictionary<string, Pool<T>>();
                }

                if (!m_pools.ContainsKey(key))
                {
                    m_pools.Add(key, new Pool<T>(reference, initialSize));
                    JSLDebug.LogFormat("[PoolCollections] - There is no pool for type '{0}' of key '{1}', create a new one.", typeof(T).Name, key);
                }
            }
        }
    }
}