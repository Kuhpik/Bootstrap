using Kuhpik;
using UnityEngine;

public class TestPauseSystem : GameSystem, IIniting, IUpdating
{
    void IIniting.OnInit()
    {
        Debug.Log($"<color=yellow>Pause state inited. Frame {Time.frameCount}</color>");
    }

    void IUpdating.OnUpdate()
    {
        Debug.Log($"Pause state update. Frame {Time.frameCount}");
    }
}
