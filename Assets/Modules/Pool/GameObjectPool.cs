using UnityEngine;

namespace JSLCore.Pool
{
    public class GameObjectPool : BasePool<GameObject>
    {
        public GameObjectPool(string key, GameObject reference, int initialSize) : base(key, reference, initialSize)
        {

        }

        public override GameObject Get()
        {
            if(m_pool.Count <= 0)
            {
                Spawn(PoolManager.Instance.DEFAULT_SPAWN_SIZE);
            }

            GameObject asset = m_pool.Dequeue();
            asset.name = m_key;
            asset.transform.SetParent(null, false);
            asset.SetActive(true);

            return asset;
        }

        public override void Recycle(GameObject asset)
        {
            asset.transform.SetParent(m_root.transform, false);
            asset.SetActive(false);

            m_pool.Enqueue(asset);
        }

    }
}
