using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuhpik
{
    public sealed partial class GameState
    {
        public class Data
        {
            public IGameSystem[] GameStartingSystems;
            public IGameSystem[] StateEnteringSystems;
            public IGameSystem[] InitingSystems;
            public IGameSystem[] UpdatingSystems;
            public IGameSystem[] LateUpdatingSystems;
            public IGameSystem[] FixedUpdatingSystems;
            public IGameSystem[] TickingSystems;
            public IGameSystem[] StateExitingSystems;
            public IGameSystem[] GameEndingSystems;

            public Data(IEnumerable<IGameSystem> systems)
            {
                PrepareCollections(systems);
            }

            async void PrepareCollections(IEnumerable<IGameSystem> systems)
            {
                GameStartingSystems     = systems.Where(x => IsOverride(x, nameof(x.OnGameStart))).ToArray();
                StateEnteringSystems    = systems.Where(x => IsOverride(x, nameof(x.OnStateEnter))).ToArray();
                InitingSystems          = systems.Where(x => IsOverride(x, nameof(x.OnInit))).ToArray();                
                StateExitingSystems     = systems.Where(x => IsOverride(x, nameof(x.OnStateExit))).ToArray();
                GameEndingSystems       = systems.Where(x => IsOverride(x, nameof(x.OnGameEnd))).ToArray();

                UpdatingSystems         = new IGameSystem[0];
                LateUpdatingSystems     = new IGameSystem[0];
                FixedUpdatingSystems    = new IGameSystem[0];
                TickingSystems          = new IGameSystem[0];

                await Task.Yield();

                UpdatingSystems         = systems.Where(x => IsOverride(x, nameof(x.OnUpdate))).ToArray();
                LateUpdatingSystems     = systems.Where(x => IsOverride(x, nameof(x.OnLateUpdate))).ToArray();
                FixedUpdatingSystems    = systems.Where(x => IsOverride(x, nameof(x.OnFixedUpdate))).ToArray();
                TickingSystems          = systems.Where(x => IsOverride(x, nameof(x.OnCustomTick))).ToArray();
            }

            bool IsOverride(IGameSystem system, string methodName)
            {
                var methodInfo = system.GetType().GetMethod(methodName);
                return methodInfo.DeclaringType != typeof(GameSystem);
            }
        }
    }
}