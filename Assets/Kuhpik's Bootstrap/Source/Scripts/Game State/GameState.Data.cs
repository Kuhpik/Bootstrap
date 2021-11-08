using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kuhpik
{
    public sealed partial class GameState
    {
        public class Data
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

            public Data(IEnumerable<IGameSystem> systems)
            {
                StartingSystems         = systems.Where(x => IsOverride(x, "OnGameStart"    )).ToArray();
                StateEnteringSystems    = systems.Where(x => IsOverride(x, "OnStateEnter"   )).ToArray();
                InitingSystems          = systems.Where(x => IsOverride(x, "OnInit"         )).ToArray();
                UpdatingSystems         = systems.Where(x => IsOverride(x, "OnUpdate"       )).ToArray();
                LateUpdatingSystems     = systems.Where(x => IsOverride(x, "OnLateUpdate"   )).ToArray();
                FixedUpdatingSystems    = systems.Where(x => IsOverride(x, "OnFixedUpdate"  )).ToArray();
                TickingSystems          = systems.Where(x => IsOverride(x, "OnCustomTick"   )).ToArray();
                StateExitingSystems     = systems.Where(x => IsOverride(x, "OnStateExit"    )).ToArray();
                GameEndingSystems       = systems.Where(x => IsOverride(x, "OnGameEnd"      )).ToArray();
            }

            bool IsOverride(IGameSystem system, string methodName)
            {
                var methodInfo = system.GetType().GetMethod(methodName);
                return methodInfo.GetBaseDefinition() != methodInfo;
            }
        }
    }
}