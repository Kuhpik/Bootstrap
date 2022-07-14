using System.Collections.Generic;
using System.Linq;

namespace Kuhpik
{
    public sealed partial class GameState
    {
        readonly public GameStateID ID;
        public GameState[] StatesIncludingSubstates { get; private set; }
        public IGameSystem[] Systems;
        readonly Data data;

        public GameState(GameStateID id, IEnumerable<IGameSystem> systems)
        {
            ID = id;
            Systems = systems.ToArray();
            StatesIncludingSubstates = new GameState[] { this };
            data = new Data(Systems);
        }

        public void JoinStates(IEnumerable<GameState> statesInTheBegining, IEnumerable<GameState> statesInTheEnd)
        {
            var connectedStates = new List<GameState>();

            connectedStates.AddRange(statesInTheBegining);
            connectedStates.AddRange(StatesIncludingSubstates);
            connectedStates.AddRange(statesInTheEnd);

            StatesIncludingSubstates = connectedStates.ToArray();
        }
    }
}