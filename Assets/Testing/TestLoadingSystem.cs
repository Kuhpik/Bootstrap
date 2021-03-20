using Kuhpik;
using UnityEngine;

public class TestLoadingSystem : GameSystem, IIniting, IUpdating
{
    void IIniting.OnInit()
    {
        Debug.Log($"<color=yellow>Loading state inited. Frame {Time.frameCount}</color>");

        Bootstrap.ChangeGameState(EGamestate.Game);
    }

    void IUpdating.OnUpdate()
    {
        Debug.Log($"Loading state update. Frame {Time.frameCount}");
    }
}
