namespace Kuhpik
{
    public interface IGameSystem
    {
        void OnGameStart();
        void OnStateEnter();
        void OnInit();
        void OnUpdate();
        void OnLateUpdate();
        void OnFixedUpdate();
        void OnCustomTick();
        void OnStateExit();
        void OnGameEnd();
    }
}