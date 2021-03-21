using Kuhpik;
using UnityEngine;

public class TestBegin2System : GameSystem, IIniting, IUpdating
{
    void IIniting.OnInit()
    {
        Debug.Log("Begin 2 inited");
    }

    void IUpdating.OnUpdate()
    {
        Debug.Log("Begin 2 updated");
    }
}
