using SparkCore.Runtime.Core;
using UnityEngine;

namespace Stateless.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : InjectableMonoBehaviour
    {
        private Animator _animator;
        private PlayerMovement playerMovement;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private readonly float sensitivity = 0.05f;

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            var playerMovementSpeed = playerMovement.Speed;
            _animator.SetFloat(Speed, playerMovementSpeed <= sensitivity ? 0 : playerMovementSpeed);
        }
    }
}