using SparkCore.Runtime.Injection;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : InjectableMonoBehaviour
    {
        private Animator _animator;
        PlayerInput _playerInput;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private readonly float sensitivity = 0.05f;

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            // var input = _playerInput.Move;
            // _animator.SetFloat(Speed, input.magnitude <= sensitivity ? 0 : input.magnitude);
        }
    }
}