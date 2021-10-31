namespace Kuhpik
{
    public interface IInstaller
    {
        int Order { get; }
        void Process();
    }
}