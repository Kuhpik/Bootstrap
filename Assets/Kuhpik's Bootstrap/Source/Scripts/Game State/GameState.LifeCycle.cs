namespace Kuhpik
{
    public partial class GameState
    {
        public void GameStart()
        {
            for (int i = 0; i < data.StartingSystems.Length; i++)
            {
                data.StartingSystems[i].OnGameStart();
            }
        }

        public void GameEnd()
        {
            for (int i = 0; i < data.GameEndingSystems.Length; i++)
            {
                data.GameEndingSystems[i].OnGameEnd();
            }
        }

        public void EnterState(GameStateID ID)
        {
            if (this.ID == ID)
            {
                Init();

                for (int i = 0; i < data.StateEnteringSystems.Length; i++)
                {
                    data.StateEnteringSystems[i].OnStateEnter();
                }
            }
        }

        public void ExitState(GameStateID ID)
        {
            if (this.ID == ID)
            {
                for (int i = 0; i < data.StateExitingSystems.Length; i++)
                {
                    data.StateExitingSystems[i].OnStateExit();
                }
            }
        }

        public void Init()
        {
            if (isInited) return;

            for (int i = 0; i < data.InitingSystems.Length; i++)
            {
                data.InitingSystems[i].OnInit();
            }

            isInited = true;
        }

        public void Update()
        {
            for (int i = 0; i < data.UpdatingSystems.Length; i++)
            {
                data.UpdatingSystems[i].OnUpdate();
            }
        }

        public void LateUpdate()
        {
            for (int i = 0; i < data.LateUpdatingSystems.Length; i++)
            {
                data.LateUpdatingSystems[i].OnLateUpdate();
            }
        }

        public void FixedUpdate()
        {
            for (int i = 0; i < data.FixedUpdatingSystems.Length; i++)
            {
                data.FixedUpdatingSystems[i].OnFixedUpdate();
            }
        }

        public void CustomTick()
        {
            for (int i = 0; i < data.TickingSystems.Length; i++)
            {
                data.TickingSystems[i].OnCustomTick();
            }
        }
    }
}