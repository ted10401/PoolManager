
namespace JSLCore.Pool
{
    public static class PoolUtility
    {
        public static T GetPooledObject<T>(this T reference) where T : class, new ()
        {
            return PoolManager.Instance.Get(reference);
        }

        public static void RecyclePooledObject<T>(this T reference) where T : class, new ()
        {
            PoolManager.Instance.Recycle(reference);
        }
    }
}