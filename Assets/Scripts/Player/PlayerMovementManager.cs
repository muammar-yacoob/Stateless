﻿using System.Collections.Generic;
using SparkCore.Runtime.Injection;
using UnityEngine;

namespace Player
{
    [RuntimeObject(RuntimeObjectType.Singleton)]
    public class PlayerMovementManager : IPlayerMovementManager
    {
        private readonly Dictionary<int, IPlayerMovement> playerMovements = new();

        public void RegisterPlayer(int index, IPlayerMovement player)
        {
            playerMovements[index] = player;
        }

        public void SetInput(int index, Vector2 input)
        {
            if (playerMovements.TryGetValue(index, out var player))
            {
                player.SetInput(input);
            }
        }

        public void Jump(int playerIndex)
        {
            if (playerMovements.TryGetValue(playerIndex, out var player))
            {
                player.Jump();
            }
        }
    }
}