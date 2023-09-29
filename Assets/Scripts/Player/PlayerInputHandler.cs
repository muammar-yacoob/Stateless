using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInput playerInput;
        private PlayerMovement playerMovement;
        private InputAction moveAction;
        private InputAction jumpAction;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            GetPlayer();
            
            moveAction = playerInput.actions.FindAction("Move");
            jumpAction = playerInput.actions.FindAction("Jump");

            jumpAction.started += OnInput;
            jumpAction.performed += OnInput;
            jumpAction.canceled += OnInput;
        }

        private void GetPlayer()
        {
            var players = FindObjectsOfType<PlayerMovement>();
            int index = playerInput.playerIndex;
            playerMovement = players.FirstOrDefault(m => m.PlayerIndex == index);
        }

        private void Update()
        {
            if(playerMovement == null)
            {
                GetPlayer();
                return;
            }
            
            if(moveAction.ReadValue<Vector2>() == Vector2.zero) return;
            playerMovement.SetInput(moveAction.ReadValue<Vector2>());
        }

        private void OnInput(InputAction.CallbackContext ctx)
        {
            switch (ctx.action.name)
            {
                case "Move":
                    //Handled in Update
                    break;
                case "Jump":
                    if (ctx.performed)
                    {
                        playerMovement.Jump();
                    }
                    break;
            }
        }
    }
}