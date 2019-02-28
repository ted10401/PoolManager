
namespace JSLCore.Pool
{
    public static class PoolUtility
    {
        public static void Recycle<T>(this T reference) where T : class, new ()
        {
            PoolManager.Instance.Recycle(reference);
        }
    }
}