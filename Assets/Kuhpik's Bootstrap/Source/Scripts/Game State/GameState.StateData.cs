using System.Collections.Generic;

namespace Kuhpik
{
    public sealed partial class GameState
    {
        public class StateData
        {
            readonly public IGameSystem[] StartingSystems;
            readonly public IGameSystem[] StateEnteringSystems;
            readonly public IGameSystem[] InitingSystems;

            readonly public IGameSystem[] UpdatingSystems;
            readonly public IGameSystem[] LateUpdatingSystems;
            readonly public IGameSystem[] FixedUpdatingSystems;
            readonly public IGameSystem[] TickingSystems;

            readonly public IGameSystem[] StateExitingSystems;
            readonly public IGameSystem[] GameEndingSystems;

            public StateData(IEnumerable<IGameSystem> systems)
            { 
                
            }
        }
    }
}