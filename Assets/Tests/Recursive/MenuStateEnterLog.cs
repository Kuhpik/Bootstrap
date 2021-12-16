using Kuhpik;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStateEnterLog : GameSystem
{
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Debug.Log("State enter menu");
    }
}
