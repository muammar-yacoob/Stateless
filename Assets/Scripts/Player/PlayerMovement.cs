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
        StatelessControlls _statelessControlls;
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake()
        {
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
            Vector3 move = transform.right * input.x + transform.forward * input.y;
            _characterController.Move(move * moveSpeed * Time.deltaTime);

            if (input.magnitude > 0)
            {
                Quaternion newDirection = Quaternion.LookRotation(new Vector3(input.x, 0, input.y));
                transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, turnSpeed * Time.deltaTime);
            }

            _animator.SetFloat(Speed, input.magnitude);
        }
    }
}