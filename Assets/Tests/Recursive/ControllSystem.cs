using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kuhpik.Framework.Tests
{
    public class ControllSystem : GameSystem
    {
        public override void OnGameStart()
        {
            this.DisplayTestMessage("OnGameStart");
        }

        public override void OnInit()
        {
            this.DisplayTestMessage("OnInit");
        }

        public override void OnStateEnter()
        {
            this.DisplayTestMessage("OnStateEnter");
        }

        public override void OnStateExit()
        {
            this.DisplayTestMessage("OnStateExit");
        }

        public override void OnGameEnd()
        {
            this.DisplayTestMessage("OnGameEnd");
        }

        public override void OnUpdate()
        {
            this.DisplayTestMessage("OnUpdate");

            if (Input.GetKeyUp(KeyCode.Space))
            {
                Bootstrap.Instance.ChangeGameState(GameStateID.Menu);
            }
        }

        public override void OnFixedUpdate()
        {
            this.DisplayTestMessage("OnFixedUpdate");
        }

        public override void OnLateUpdate()
        {
            this.DisplayTestMessage("OnLateUpdate");
        }
    }
}
