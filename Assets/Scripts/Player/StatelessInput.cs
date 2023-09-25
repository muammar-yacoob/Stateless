using System;
using SparkCore.Runtime.Injection;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Player
{
    [Injectable(Lifetime.Singleton)]
    public class StatelessInput : IPlayerInput
    {
        private StatelessControlls _statelessControlls;
        public Vector2 Move => _statelessControlls.gameplay.move.ReadValue<Vector2>();
        
        public event Action Jump;
        
        public StatelessInput()
        {
            _statelessControlls = new StatelessControlls();
            _statelessControlls.gameplay.Enable();
            
            _statelessControlls.gameplay.Jump.performed += JumpPerformed;
            //_ = _statelessControlls.gameplay.Jump.interactions.Insert(0, "Press"); // Insert Press interaction at the beginning of the list to avoid the Hold interaction
        }


        public void Dispose()
        {
            _statelessControlls.gameplay.Jump.performed -= JumpPerformed;
            _statelessControlls.gameplay.Disable();
        }
        
        private void JumpPerformed(InputAction.CallbackContext ctx) => Jump?.Invoke();
    }
}