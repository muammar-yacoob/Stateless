using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private float moveSpeed =5f;

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontal, 0.0f, vertical);
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}