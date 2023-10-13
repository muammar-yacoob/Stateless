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
        
        private const string MOVE_ACTION = "Move";
        private const string JUMP_ACTION = "Jump";
        private const string FIRE_ACTION = "Fire";

        protected override void Awake()
        {
            base.Awake();
            playerInput = GetComponent<PlayerInput>();
            playerIndex = playerInput.playerIndex;

            //Movement
            moveAction = playerInput.actions.FindAction(MOVE_ACTION);
            //ReadValue<Vector2>() is not called here because it is called in Update()
            
            //Jump
            jumpAction = playerInput.actions.FindAction(JUMP_ACTION);
            jumpAction.performed += OnInput;
            
            //Fire
            fireAction = playerInput.actions.FindAction(FIRE_ACTION);
            fireAction.performed += OnInput;
        }

        private void Update()
        {
            if (moveAction == null) return;
            //if (moveAction.ReadValue<Vector2>() == Vector2.zero) return; //Idle animation needs the zero value
            playerMovementManager.SetInput(playerIndex, moveAction.ReadValue<Vector2>());
        }

        private void OnInput(InputAction.CallbackContext ctx)
        {
            //Jump
            if(!ctx.performed) return;
            switch (ctx.action.name)
            {
                case JUMP_ACTION:
                    playerMovementManager.Jump(playerIndex);
                    break;
                case FIRE_ACTION:
                    playerMovementManager.Fire(playerIndex);
                    break;
            }
        }
    }
}