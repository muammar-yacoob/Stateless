using UnityEngine;

namespace Player
{
    public class GroundCharacter : MonoBehaviour
    {
        [SerializeField] private float raycastDistance = 1.0f; 
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Vector3 characterGroundOffset;
        
        private readonly Vector3 rayOffset = Vector3.up * 0.3f;

        private void Update()
        {
            if (Physics.Raycast(transform.position + rayOffset, Vector3.down, out RaycastHit hit, raycastDistance, groundLayer))
            {
                transform.position = hit.point + characterGroundOffset;
            }
        }
    }
}