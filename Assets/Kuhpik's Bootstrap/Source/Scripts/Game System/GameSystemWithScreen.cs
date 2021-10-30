namespace Kuhpik
{
    public abstract class GameSystemWithScreen<T> : GameSystem where T : UIScreen
    {
        protected T screen;

        void Start()
        {
            screen = UIManager.GetUIScreen<T>();
        }
    }
}