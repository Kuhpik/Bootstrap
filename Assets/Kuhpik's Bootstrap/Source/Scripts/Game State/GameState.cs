using System.Collections.Generic;
using System.Linq;

namespace Kuhpik
{
    public sealed partial class GameState
    {
        readonly public GameStateID ID;
        public IGameSystem[] Systems;
        public GameState[] StatesIncludingSubstates { get; private set; }

        Data data;
        bool isInited;

        public GameState(GameStateID id, IEnumerable<IGameSystem> systems)
        {
            ID = id;
            Systems = systems.ToArray();
            StatesIncludingSubstates = new GameState[] { this };
            Setup();
        }

        public void ContactStates(IEnumerable<GameState> statesInTheBegining, IEnumerable<GameState> statesInTheEnd)
        {
            var contacted = new List<GameState>();

            contacted.AddRange(statesInTheBegining);
            contacted.AddRange(StatesIncludingSubstates);
            contacted.AddRange(statesInTheEnd);

            StatesIncludingSubstates = contacted.ToArray();
            Setup();
        }

        void Setup()
        {
            var systems = new List<IGameSystem>();

            for (int i = 0; i < StatesIncludingSubstates.Length; i++)
            {
                for (int j = 0; j < StatesIncludingSubstates[i].Systems.Length; j++)
                {
                    systems.Add(StatesIncludingSubstates[i].Systems[j]);
                }
            }

            data = new Data(systems);
        }
    }
}