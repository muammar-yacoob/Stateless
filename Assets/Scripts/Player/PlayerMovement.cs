using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float turnSpeed = 20;
        private CharacterController _characterController;
        private Animator _animator;
        private Transform cameraTransform;
        StatelessControlls _statelessControlls;
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake()
        {
            cameraTransform = Camera.main.transform;
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();

            _statelessControlls = new StatelessControlls();
            _statelessControlls.gameplay.Enable();
            _statelessControlls.gameplay.move.performed += OnMove;
            _statelessControlls.gameplay.move.canceled += OnMove;  // Subscribe to the canceled event
        }

        private void OnDestroy()
        {
            _statelessControlls.gameplay.move.performed -= OnMove;
            _statelessControlls.gameplay.move.canceled -= OnMove;  // Unsubscribe from the canceled event
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            MoveCharacter(input);
        }

        private void MoveCharacter(Vector2 input)
        {
            // Take the camera's orientation into account for movement
            Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 moveDirection = (cameraForward * input.y + cameraTransform.right * input.x).normalized;

            // Move the character
            Vector3 move = moveDirection * moveSpeed * Time.deltaTime;
            _characterController.Move(move);

            // Rotate the character based on input direction
            if (input.magnitude > 0)
            {
                Quaternion newDirection = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, turnSpeed * Time.deltaTime);
            }

            _animator.SetFloat(Speed, input.magnitude);
        }
    }
}