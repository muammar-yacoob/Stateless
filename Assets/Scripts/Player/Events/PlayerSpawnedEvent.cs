using UnityEngine;

namespace Stateless.Player.Events
{
    public class PlayerSpawnedEvent
    {
        public readonly GameObject PlayerInstance;

        public PlayerSpawnedEvent(GameObject playerInstance)
        {
            PlayerInstance = playerInstance;
        }

    }
}