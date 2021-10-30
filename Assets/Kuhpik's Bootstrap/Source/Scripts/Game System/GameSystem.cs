﻿using UnityEngine;

namespace Kuhpik
{
    public abstract partial class GameSystem : MonoBehaviour, IGameSystem
    {
        protected PlayerData player;
        protected GameConfig config;
        protected GameData game;

        public virtual void OnCustomTick() { }

        public virtual void OnFixedUpdate() { }

        public virtual void OnGameExit() { }

        public virtual void OnGameStart() { }

        public virtual void OnInit() { }

        public virtual void OnLateUpdate() { }

        public virtual void OnStateEnter() { }

        public virtual void OnStateExit() { }

        public virtual void OnUpdate() { }
    }
}