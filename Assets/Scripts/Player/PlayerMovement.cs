﻿using SparkCore.Runtime.Core;
using SparkCore.Runtime.Injection;
using UnityEngine;

namespace Stateless.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerJump))]
    public class PlayerMovement : InjectableMonoBehaviour, IPlayerMovement
    {
        [SerializeField] private int playerIndex;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float turnSpeed = 20f; // Add turn speed

        public int PlayerIndex => playerIndex;
        private float speed;
        public float Speed => speed;
        
        private CharacterController _characterController;
        private Transform _cameraTransform;
        private PlayerJump playerJump;
        private PlayerFire playerFire;
        
        [Inject] IPlayerMovementManager playerMovementManager;

        protected override void Awake()
        {
            base.Awake();
            _characterController = GetComponent<CharacterController>();
            _cameraTransform = Camera.main?.transform;
            playerJump = GetComponent<PlayerJump>();
            playerFire = GetComponent<PlayerFire>();
            
            playerMovementManager.RegisterPlayer(this);
        }
        
        private void OnDestroy()
        {
            playerMovementManager.RemovePlayer(this);
        }

        public void DestroyPlayer()
        {
            playerMovementManager.RemovePlayer(this);
            Destroy(gameObject);
        }
        
        public void SetInput(Vector2 input)
        {
            speed = input.magnitude;
            if(input.magnitude == 0) return;

            Vector3 moveDirection = _cameraTransform.forward * input.y + _cameraTransform.right * input.x;
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

        public void Jump() => playerJump.Jump();
        
        public void Fire() => playerFire.Fire();
    }
}