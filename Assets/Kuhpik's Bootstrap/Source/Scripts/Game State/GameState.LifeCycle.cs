namespace Kuhpik
{
    public partial class GameState
    {
        public void GameStart()
        {
            for (int i = 0; i < StatesIncludingSubstates.Length; i++)
            {
                for (int j = 0; j < StatesIncludingSubstates[i].data.GameStartingSystems.Length; j++)
                {
                    StatesIncludingSubstates[i].data.GameStartingSystems[j].OnGameStart();
                }

                // No more game start calls
                StatesIncludingSubstates[i].data.GameStartingSystems = new IGameSystem[0];
            }
        }

        public void Init()
        {
            for (int i = 0; i < StatesIncludingSubstates.Length; i++)
            {
                for (int j = 0; j < StatesIncludingSubstates[i].data.InitingSystems.Length; j++)
                {
                    StatesIncludingSubstates[i].data.InitingSystems[j].OnInit();
                }

                // No more Init calls
                StatesIncludingSubstates[i].data.InitingSystems = new IGameSystem[0];
            }
        }

        public void GameEnd()
        {
            for (int i = 0; i < StatesIncludingSubstates.Length; i++)
            {
                for (int j = 0; j < StatesIncludingSubstates[i].data.GameEndingSystems.Length; j++)
                {
                    StatesIncludingSubstates[i].data.GameEndingSystems[j].OnGameEnd();
                }

                // No more game end calls
                StatesIncludingSubstates[i].data.GameEndingSystems = new IGameSystem[0];
            }
        }

        public void EnterState(GameStateID ID)
        {
            if (this.ID == ID)
            {
                Init();

                for (int i = 0; i < this.data.StateEnteringSystems.Length; i++)
                {
                    this.data.StateEnteringSystems[i].OnStateEnter();
                }
            }
        }

        public void ExitState(GameStateID ID)
        {
            if (this.ID == ID)
            {
                for (int i = 0; i < this.data.StateExitingSystems.Length; i++)
                {
                    this.data.StateExitingSystems[i].OnStateExit();
                }
            }
        }

        public void Update()
        {
            for (int i = 0; i < StatesIncludingSubstates.Length; i++)
            {
                for (int j = 0; j < StatesIncludingSubstates[i].data.UpdatingSystems.Length; j++)
                {
                    StatesIncludingSubstates[i].data.UpdatingSystems[j].OnUpdate();
                }
            }
        }

        public void LateUpdate()
        {
            for (int i = 0; i < StatesIncludingSubstates.Length; i++)
            {
                for (int j = 0; j < StatesIncludingSubstates[i].data.LateUpdatingSystems.Length; j++)
                {
                    StatesIncludingSubstates[i].data.LateUpdatingSystems[j].OnLateUpdate();
                }
            }
        }

        public void FixedUpdate()
        {
            for (int i = 0; i < StatesIncludingSubstates.Length; i++)
            {
                for (int j = 0; j < StatesIncludingSubstates[i].data.FixedUpdatingSystems.Length; j++)
                {
                    StatesIncludingSubstates[i].data.FixedUpdatingSystems[j].OnFixedUpdate();
                }
            }
        }

        // public void CustomTick()
        // {
        //     for (int i = 0; i < StatesIncludingSubstates.Length; i++)
        //     {
        //         for (int j = 0; j < StatesIncludingSubstates[i].data.TickingSystems.Length; j++)
        //         {
        //             StatesIncludingSubstates[i].data.TickingSystems[j].OnCustomTick();
        //         }
        //     }
        // }
    }
}