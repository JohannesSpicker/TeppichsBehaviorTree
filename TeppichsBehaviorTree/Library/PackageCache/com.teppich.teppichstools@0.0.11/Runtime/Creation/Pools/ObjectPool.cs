using System.Collections.Generic;

namespace TeppichsTools.Creation.Pools
{
    public class ObjectPool<T> : IObjectPool<T> where T : new()
    {
        public readonly List<T> free  = new List<T>();
        public readonly List<T> inUse = new List<T>();
        public readonly List<T> pool  = new List<T>();

        public T Next()
        {
            foreach (T candidate in free)
            {
                free.Remove(candidate);
                inUse.Add(candidate);

                return candidate;
            }

            T instanced = new T();
            pool.Add(instanced);
            inUse.Add(instanced);

            return instanced;
        }

        public void Release(T released)
        {
            free.Add(released);
            inUse.Remove(released);
        }

        public void Cull() => free.Clear();
    }
}