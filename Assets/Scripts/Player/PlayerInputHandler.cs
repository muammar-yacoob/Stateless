using SparkCore.Runtime.Injection;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Player
{
    public class PlayerInputHandler : InjectableMonoBehaviour
    {
        [Inject] IPlayerMovementManager playerMovementManager;
        
        private PlayerInput playerInput;
        private int playerIndex;
        private InputAction moveAction;
        private InputAction jumpAction;

        protected override void Awake()
        {
            base.Awake();
            playerInput = GetComponent<PlayerInput>();
            playerIndex = playerInput.playerIndex;

            moveAction = playerInput.actions.FindAction("Move");
            jumpAction = playerInput.actions.FindAction("Jump");

            jumpAction.started += OnInput;
            jumpAction.performed += OnInput;
            jumpAction.canceled += OnInput;
        }

        private void Update()
        {
            if(moveAction == null) return;
            if (moveAction.ReadValue<Vector2>() == Vector2.zero) return;
            playerMovementManager.SetInput(playerIndex, moveAction.ReadValue<Vector2>());
        }

        private void OnInput(InputAction.CallbackContext ctx)
        {
            if (ctx.action == jumpAction && ctx.performed)
            {
                playerMovementManager.Jump(playerIndex);
            }
        }
    }
}