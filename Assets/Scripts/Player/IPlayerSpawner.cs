using UnityEngine.InputSystem;

namespace Player
{
    public interface IPlayerSpawner
    {
        void SpawnPlayer(PlayerInput playerInput);
    }
}