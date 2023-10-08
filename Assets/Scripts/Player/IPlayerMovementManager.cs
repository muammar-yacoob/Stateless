using UnityEngine;

namespace Stateless.Player
{

    public interface IPlayerMovementManager
    {
        void RegisterPlayer(int index, IPlayerMovement player);
        void SetInput(int index, Vector2 input);
        void Jump(int playerIndex);
    }
}