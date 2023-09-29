using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerJump))]
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        [SerializeField] private int playerIndex;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float turnSpeed = 20f; // Add turn speed

        public int PlayerIndex => playerIndex;
        
        private CharacterController _characterController;
        private Transform _cameraTransform;
        private PlayerJump playerJump;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _cameraTransform = Camera.main.transform;
            playerJump = GetComponent<PlayerJump>();
        }

        public void SetInput(Vector2 input)
        {
            if(input.magnitude == 0) return;

            Vector3 moveDirection = _cameraTransform.forward * input.y + _cameraTransform.right * input.x;
            moveDirection.y = 0;
            moveDirection.Normalize();

            // Rotate the character to face the move direction
            if (moveDirection != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
            }
            _characterController.Move(moveDirection * (moveSpeed * Time.deltaTime));
        }

        public void Jump() => playerJump.Jump();
    }
}