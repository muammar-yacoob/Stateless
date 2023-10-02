using UnityEngine.InputSystem;

namespace Stateless.Player
{
    public interface IPlayerSpawner
    {
        void SpawnPlayer(PlayerInput playerInput);
    }
}