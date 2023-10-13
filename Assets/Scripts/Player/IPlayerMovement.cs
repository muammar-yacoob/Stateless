using UnityEngine;

namespace Stateless.Player
{
    public interface IPlayerMovement
    {
        void SetInput(Vector2 input);
        void Jump();
        void Fire();
    }
}