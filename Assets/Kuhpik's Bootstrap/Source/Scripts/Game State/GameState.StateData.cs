using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
                StartingSystems = systems.Where(x => IsOverride(x.GetType().GetMethod("OnGameStart"))).ToArray();
                StateEnteringSystems = systems.Where(x => IsOverride(x.GetType().GetMethod("OnStateEnter"))).ToArray();
                InitingSystems = systems.Where(x => IsOverride(x.GetType().GetMethod("OnInit"))).ToArray();

                UpdatingSystems = systems.Where(x => IsOverride(x.GetType().GetMethod("OnUpdate"))).ToArray();
                LateUpdatingSystems = systems.Where(x => IsOverride(x.GetType().GetMethod("OnLateUpdate"))).ToArray();
                FixedUpdatingSystems = systems.Where(x => IsOverride(x.GetType().GetMethod("OnFixedUpdate"))).ToArray();
                TickingSystems = systems.Where(x => IsOverride(x.GetType().GetMethod("OnCustomTick"))).ToArray();

                StateExitingSystems = systems.Where(x => IsOverride(x.GetType().GetMethod("OnStateExit"))).ToArray();
                GameEndingSystems = systems.Where(x => IsOverride(x.GetType().GetMethod("OnGameEnd"))).ToArray();
            }

            bool IsOverride(MethodInfo methodInfo)
            {
                return methodInfo.GetBaseDefinition() != methodInfo;
            }
        }
    }
}