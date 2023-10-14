using UnityEngine;

namespace Stateless.Player
{

    public interface IPlayerMovementManager
    {
        void RegisterPlayer(IPlayerMovement player);
        void RemovePlayer(IPlayerMovement player);
        void SetInput(int index, Vector2 input);
        void Jump(int playerIndex);
        void Fire(int playerIndex);
    }
}