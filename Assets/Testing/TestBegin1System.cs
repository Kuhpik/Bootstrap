using Kuhpik;
using UnityEngine;

public class TestBegin1System : GameSystem, IIniting, IUpdating
{
    void IIniting.OnInit()
    {
        Debug.Log("Begin 1 inited");
    }

    void IUpdating.OnUpdate()
    {
        Debug.Log("Begin 1 updated");
    }
}
