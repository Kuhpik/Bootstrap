using Kuhpik;
using UnityEngine;

public class TestGameSystem : GameSystem, IIniting, IUpdating
{
    void IIniting.OnInit()
    {
        Debug.Log($"<color=yellow>Game state inited. Frame {Time.frameCount}</color>");
    }

    void IUpdating.OnUpdate()
    {
        Debug.Log($"Game state update. Frame {Time.frameCount}");

        if (Input.GetKeyDown(KeyCode.Space)) Bootstrap.ChangeGameState(GameStateName.Loading);
    }
}
