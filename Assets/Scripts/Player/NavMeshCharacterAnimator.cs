using UnityEngine;
using UnityEngine.AI;

namespace Stateless.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class NavMeshCharacterAnimator : MonoBehaviour
    {
        [SerializeField] private float turnSpeed = 20f;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            float speed = _navMeshAgent.velocity.magnitude;
            _animator.SetFloat(Speed, speed > 0.1f ? speed : 0);
        }
    }
}