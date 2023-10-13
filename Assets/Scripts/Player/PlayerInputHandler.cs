using SparkCore.Runtime.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Stateless.Player
{
    public class PlayerInputHandler : InjectableMonoBehaviour
    {
        [Inject] IPlayerMovementManager playerMovementManager;

        private PlayerInput playerInput;
        private int playerIndex;
        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction fireAction;

        protected override void Awake()
        {
            base.Awake();
            playerInput = GetComponent<PlayerInput>();
            playerIndex = playerInput.playerIndex;

            Debug.Log($"Mapping input for Player {playerIndex}");

            moveAction = playerInput.actions.FindAction("Move");
            
            //Jump
            jumpAction = playerInput.actions.FindAction("Jump");
            jumpAction.started += OnInput;
            jumpAction.performed += OnInput;
            jumpAction.canceled += OnInput;
            
            //Fire
            fireAction = playerInput.actions.FindAction("Fire");
            fireAction.started += OnInput;
            fireAction.performed += OnInput;
            fireAction.canceled += OnInput;
        }

        private void Update()
        {
            if (moveAction == null) return;
            //if (moveAction.ReadValue<Vector2>() == Vector2.zero) return; //Idle animation needs the zero value
            playerMovementManager.SetInput(playerIndex, moveAction.ReadValue<Vector2>());
        }

        private void OnInput(InputAction.CallbackContext ctx)
        {
            if (ctx.action == jumpAction && ctx.performed)
            {
                playerMovementManager.Jump(playerIndex);
            }

            if (ctx.action == fireAction && ctx.performed)
            {
                playerMovementManager.Fire(playerIndex);
            }
        }
    }
}