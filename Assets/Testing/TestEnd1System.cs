using Kuhpik;
using UnityEngine;

public class TestEnd1System : GameSystem, IIniting, IUpdating
{
    void IIniting.OnInit()
    {
        Debug.Log("End 1 inited");
    }

    void IUpdating.OnUpdate()
    {
        Debug.Log("End 1 updated");
    }
}
