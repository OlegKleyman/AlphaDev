namespace AlphaDev.Core.Data.Contexts
{
    public interface IConnectable<T>
    {
        T ConnectionDetails { get; }
    }
}