using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    [RequireComponent(typeof(Animator))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float turnSpeed;
        public Vector2 MoveInput => moveInput;
        
        private MajiControls controls;
        private Vector2 moveInput;
        private Animator animator;
        private static readonly int Wave = Animator.StringToHash("Wave");
        private static readonly int Speak = Animator.StringToHash("Speak");

        private void Awake()
        {
            controls = new MajiControls();
            controls.gameplay.Enable();
            controls.gameplay.move.performed += OnMoveOnperformed;
            controls.gameplay.move.canceled += OnMoveOncanceled;
            controls.gameplay.wave.performed += OnWaveOnperformed;
            controls.gameplay.Speak.performed += OnSpeakOnperformed;

            animator = GetComponent<Animator>();
        }

        private void OnDestroy()
        {
            controls.gameplay.Disable();
            controls.gameplay.move.performed -= OnMoveOnperformed;
            controls.gameplay.move.canceled -= OnMoveOncanceled;
            controls.gameplay.wave.performed -= OnWaveOnperformed;
            controls.gameplay.Speak.performed -= OnSpeakOnperformed;
        }

        #region Input Actions to animation mapping.
        private void OnSpeakOnperformed(InputAction.CallbackContext ctx)
        {
            animator.SetBool(Speak, animator.GetBool(Speak) == false);
        }

        private void OnWaveOnperformed(InputAction.CallbackContext ctx)
        {
            animator.SetTrigger(Wave);
        }

        private void OnMoveOncanceled(InputAction.CallbackContext ctx)
        {
            moveInput = Vector2.zero;
        }

        private void OnMoveOnperformed(InputAction.CallbackContext ctx)
        {
            moveInput = ctx.ReadValue<Vector2>();
        }
        #endregion


        private void Update()
        {
            var movement = new Vector3(moveInput.x, 0, moveInput.y);
            transform.Translate(movement * Time.deltaTime * moveSpeed , Space.World);
            animator.SetFloat("Speed", movement.magnitude);
            if(movement.magnitude > 0)
            {
                Quaternion newDirection = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, turnSpeed * Time.deltaTime);
            }
        }
    }
}