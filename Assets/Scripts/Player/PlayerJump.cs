using SparkCore.Runtime.Injection;
using UnityEngine;
using VContainer;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerJump : InjectableMonoBehaviour
    {
        [Header("Jump Settings")]
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float jumpDuration = 0.4f;
        [SerializeField] private int maxJumpCount = 2;

        
        
        private CharacterController _characterController;
        private Vector3 _velocity;
        private int _currentJumpCount;
        private bool _shouldJump;

        private float Gravity => -2 * jumpHeight / Mathf.Pow(jumpDuration, 2);

        protected override void Awake()
        {
            base.Awake();
            _characterController = GetComponent<CharacterController>();
            _currentJumpCount = 0;
        }

        private void Update()
        {
            if (_characterController.isGrounded)
            {
                _currentJumpCount = 0;
                _velocity.y = 0;
            }

            if (_shouldJump)
            {
                _velocity.y = Mathf.Sqrt(-2 * jumpHeight * Gravity);
                _shouldJump = false;
            }

            _velocity.y += Gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }

        public void Jump()
        {
            if (_currentJumpCount < maxJumpCount)
            {
                _currentJumpCount++;
                _shouldJump = true;
            }
        }
    }
}