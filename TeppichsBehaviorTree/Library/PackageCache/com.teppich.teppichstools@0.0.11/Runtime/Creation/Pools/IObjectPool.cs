namespace TeppichsTools.Creation.Pools
{
    public interface IObjectPool<T>
    {
        T    Next();
        void Release(T released);
    }
}