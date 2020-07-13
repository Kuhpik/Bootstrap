namespace Kuhpik
{
    internal interface IUIScreen : IUIElement
    {
        EGamestate Type { get; }
        void Open();
        void Close();
    }
}