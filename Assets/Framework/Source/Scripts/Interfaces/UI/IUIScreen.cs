internal interface IUIScreen : IUIElement
{
    UIScreenType Type { get; }
    void Open();
    void Close();
}