using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuhpik
{
    public sealed partial class GameState
    {
        public class Data
        {
            public IGameSystem[] StartingSystems { get; private set; }
            public IGameSystem[] StateEnteringSystems { get; private set; }
            public IGameSystem[] InitingSystems { get; private set; }
            public IGameSystem[] UpdatingSystems { get; private set; }
            public IGameSystem[] LateUpdatingSystems { get; private set; }
            public IGameSystem[] FixedUpdatingSystems { get; private set; }
            public IGameSystem[] TickingSystems { get; private set; }
            public IGameSystem[] StateExitingSystems { get; private set; }
            public IGameSystem[] GameEndingSystems { get; private set; }

            public Data(IEnumerable<IGameSystem> systems)
            {
                PrepareCollections(systems);
            }

            async void PrepareCollections(IEnumerable<IGameSystem> systems)
            {
                StartingSystems = systems.Where(x => IsOverride(x, "OnGameStart")).ToArray();
                StateEnteringSystems = systems.Where(x => IsOverride(x, "OnStateEnter")).ToArray();
                InitingSystems = systems.Where(x => IsOverride(x, "OnInit")).ToArray();                
                StateExitingSystems = systems.Where(x => IsOverride(x, "OnStateExit")).ToArray();
                GameEndingSystems = systems.Where(x => IsOverride(x, "OnGameEnd")).ToArray();

                UpdatingSystems = new IGameSystem[0];
                LateUpdatingSystems = new IGameSystem[0];
                FixedUpdatingSystems = new IGameSystem[0];
                TickingSystems = new IGameSystem[0];

                await Task.Yield();

                UpdatingSystems = systems.Where(x => IsOverride(x, "OnUpdate")).ToArray();
                LateUpdatingSystems = systems.Where(x => IsOverride(x, "OnLateUpdate")).ToArray();
                FixedUpdatingSystems = systems.Where(x => IsOverride(x, "OnFixedUpdate")).ToArray();
                TickingSystems = systems.Where(x => IsOverride(x, "OnCustomTick")).ToArray();
            }

            bool IsOverride(IGameSystem system, string methodName)
            {
                var methodInfo = system.GetType().GetMethod(methodName);
                return methodInfo.DeclaringType != typeof(GameSystem);
            }
        }
    }
}