using SparkCore.Runtime.Injection;
using UnityEngine;
using VContainer;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : InjectableMonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float turnSpeed = 20f; // Add turn speed
        [Inject] private IPlayerInput _playerInput;
        private CharacterController _characterController;
        private Transform _cameraTransform;

        protected override void Awake()
        {
            base.Awake();
            _characterController = GetComponent<CharacterController>();
            _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            var input = _playerInput.Move;
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
    }
}