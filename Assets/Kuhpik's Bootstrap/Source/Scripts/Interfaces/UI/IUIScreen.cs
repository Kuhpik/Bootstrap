namespace Kuhpik
{
    internal interface IUIScreen : IUIElement
    {
        GameState.Identity Type { get; }
        void Open();
        void Close();
    }
}