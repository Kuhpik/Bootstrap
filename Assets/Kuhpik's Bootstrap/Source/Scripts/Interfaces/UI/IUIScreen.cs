namespace Kuhpik
{
    internal interface IUIScreen : IUIElement
    {
        GameState.Identificator Type { get; }
        void Open();
        void Close();
    }
}