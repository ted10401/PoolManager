using UnityEngine;

namespace JSLCore.Pool
{
    public static class PoolUtility
    {
        public static void Recycle<T>(this T reference) where T : class
        {
            PoolManager.Instance.Recycle(reference);
        }

        public static string GetKey<T>(this T reference) where T : class
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

            return key;
        }

        public static PoolType GetPoolType<T>(this T reference) where T : class
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

        public static Transform GetRoot<T>(this T reference) where T : class
        {
            Transform root = null;

            if (reference is GameObject)
            {
                root = new GameObject(string.Format("[Pool ({0})] - {1}", typeof(T).Name, reference.GetKey())).transform;
                root.SetParent(PoolManager.Instance.transform);
            }
            else if (reference is Component)
            {
                root = new GameObject(string.Format("[Pool ({0})] - {1}", typeof(T).Name, reference.GetKey())).transform;
                root.SetParent(PoolManager.Instance.transform);
            }

            return root;
        }
    }
}