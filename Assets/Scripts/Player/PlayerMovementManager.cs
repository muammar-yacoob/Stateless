using System.Collections.Generic;
using SparkCore.Runtime.Injection;
using UnityEngine;

namespace Stateless.Player
{
    [RuntimeObject(RuntimeObjectType.Singleton)]
    public class PlayerMovementManager : IPlayerMovementManager
    {
        private readonly Dictionary<int, IPlayerMovement> playerMovements = new();

        public Dictionary<int, IPlayerMovement> PlayerMovements => playerMovements;

        public void RegisterPlayer(IPlayerMovement player)
        {
            //Player index in player prefabs should be set to values from 0 to 3
            playerMovements.TryAdd(playerMovements.Count, player);
            foreach (var playerMovement in playerMovements)
            {
                Debug.Log($"Player {playerMovement.Key} registered");
            }
        }

        public void RemovePlayer(IPlayerMovement player)
        {
            playerMovements.Remove(playerMovements.Count);
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

        public void Fire(int playerIndex)
        {
            if (playerMovements.TryGetValue(playerIndex, out var player))
            {
                player.Fire();
            }
        }
    }
}