using UnityEngine;
using System.Collections.Generic;

namespace JSLCore.Pool
{
    public class PoolManager : MonoSingleton<PoolManager>
	{
        public int DEFAULT_SPAWN_SIZE = 5;
        private Dictionary<string, GameObjectPool> m_gameObjectPools;

        private void Awake()
		{
			m_gameObjectPools = new Dictionary<string, GameObjectPool>();
		}

        public void AddPool(GameObject referenceAsset, int initialSize)
        {
            if (m_gameObjectPools.ContainsKey(referenceAsset.name))
            {
                return;
            }

            if (string.IsNullOrEmpty(referenceAsset.name))
            {
                JSLDebug.LogError("[ObjectPoolManager] - The key is null or empty, register fail.");
                return;
            }

            if (referenceAsset == null)
            {
                JSLDebug.LogError("[ObjectPoolManager] - The reference asset is null, register fail.");
                return;
            }

            m_gameObjectPools[referenceAsset.name] = new GameObjectPool(referenceAsset.name, referenceAsset, initialSize);
        }

        public GameObject Get(GameObject referenceAsset)
        {
            if (!HasPool(referenceAsset.name))
            {
                AddPool(referenceAsset, DEFAULT_SPAWN_SIZE);
                JSLDebug.LogErrorFormat(string.Format("[ObjectPoolManager] - ObjectPool \"{0}\" doesn't exist. Create a new one for it.", referenceAsset.name));
            }

            return m_gameObjectPools[referenceAsset.name].Get();
        }

        public void Recycle(GameObject recycleAsset)
        {
            if (!HasPool(recycleAsset.name))
            {
                JSLDebug.LogErrorFormat(string.Format("[ObjectPoolManager] - ObjectPool \"{0}\" doesn't exist. Destroy the GameObject directly", recycleAsset.name));
                GameObject.Destroy(recycleAsset);
            }

            m_gameObjectPools[recycleAsset.name].Recycle(recycleAsset);
        }

        private bool HasPool(string key)
		{
			return m_gameObjectPools.ContainsKey(key);
		}

        public void Clear()
        {
            foreach (KeyValuePair<string, GameObjectPool> kvp in m_gameObjectPools)
            {
                kvp.Value.Destroy();
            }

            m_gameObjectPools.Clear();
        }
    }
}