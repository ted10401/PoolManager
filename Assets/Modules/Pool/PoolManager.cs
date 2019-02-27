using UnityEngine;
using System.Collections.Generic;

namespace JSLCore.Pool
{
    public class PoolManager : MonoSingleton<PoolManager>
	{
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

        public void Clear()
        {

        }

        private static class PoolCollections<T> where T : class
        {
            private const int DEFAULT_INITIAL_SIZE = 5;

            private static ClassPool<T> m_defaultPool;
            private static Dictionary<string, ClassPool<T>> m_pools;

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

            public static ClassPool<T> GetPool(T reference)
            {
                string key = GetKey(reference);

                if (string.IsNullOrEmpty(key))
                {
                    InitializeDefaultPool(reference, DEFAULT_INITIAL_SIZE);
                    return m_defaultPool;
                }
                else
                {
                    InitializePools(key, reference, DEFAULT_INITIAL_SIZE);
                    return m_pools[key];
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
                    m_defaultPool = new ClassPool<T>(reference, initialSize);
                }
            }

            private static void InitializePools(string key, T reference, int initialSize)
            {
                if (m_pools == null)
                {
                    m_pools = new Dictionary<string, ClassPool<T>>();
                    m_pools.Add(key, new ClassPool<T>(reference, initialSize));
                }
                else if (!m_pools.ContainsKey(key))
                {
                    m_pools.Add(key, new ClassPool<T>(reference, initialSize));
                }
            }
        }
    }
}