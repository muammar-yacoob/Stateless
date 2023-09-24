using System;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerNavMeshController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private NavMeshAgent _navMeshAgent;
        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (target == null) return;
            _navMeshAgent.SetDestination(target.position);
        }

        // private void ClickToMove()
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //         Physics.Raycast(ray, out var hit);
        //         _navMeshAgent.SetDestination(hit.point);
        //     }
        // }
    }
}