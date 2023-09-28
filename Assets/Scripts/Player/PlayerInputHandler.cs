using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInput playerInput;
        private PlayerMovement playerMovement;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            GetPlayer();
            playerInput.onActionTriggered += OnInput;
        }

        private void GetPlayer()
        {
            var players = FindObjectsOfType<PlayerMovement>();
            int index = playerInput.playerIndex;
            playerMovement = players.FirstOrDefault(m => m.PlayerIndex == index);
        }

        private void OnInput(InputAction.CallbackContext ctx)
        {
            switch (ctx.action.name)
            {
                case "Move":
                    Vector2 moveInput = ctx.ReadValue<Vector2>();
                    playerMovement.Input = moveInput;
                    break;
                case "Jump":
                    if (ctx.performed)
                    {
                        //playerMovement.Jump();
                    }
                    break;
            }
        }
    }
}