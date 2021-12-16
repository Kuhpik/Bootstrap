using Kuhpik;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseLogSystem : GameSystem
{
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Debug.Log("Sure");
    }
}
