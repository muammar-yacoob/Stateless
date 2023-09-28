using SparkCore.Runtime.Injection;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : InjectableMonoBehaviour
    {
        [SerializeField] private int playerIndex;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float turnSpeed = 20f; // Add turn speed

        public int PlayerIndex => playerIndex;
        public Vector2 Input { get; set; }
        
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
            if(Input.magnitude == 0) return;

            Vector3 moveDirection = _cameraTransform.forward * Input.y + _cameraTransform.right * Input.x;
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