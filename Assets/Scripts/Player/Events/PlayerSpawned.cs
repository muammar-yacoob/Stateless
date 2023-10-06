using UnityEngine;

namespace Stateless.Player.Events
{
    public class PlayerSpawned
    {
        public readonly GameObject PlayerInstance;
        public readonly int PlayerIndex;

        public PlayerSpawned(GameObject playerInstance, int playerIndex)
        {
            PlayerInstance = playerInstance;
            PlayerIndex = playerIndex;
        }
    }
}