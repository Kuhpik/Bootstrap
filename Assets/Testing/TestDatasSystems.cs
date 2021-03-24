using Kuhpik;
using UnityEngine;

public class TestDatasSystems : GameSystem, IIniting
{
    void IIniting.OnInit()
    {
        Debug.Log(player);
        Debug.Log(game);
    }
}
