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
        
        public event Action Speak;
        
        public StatelessInput()
        {
            _statelessControlls = new StatelessControlls();
            _statelessControlls.gameplay.Enable();
            
            _statelessControlls.gameplay.Speak.performed += SpeakPerformed;
        }


        public void Dispose()
        {
            _statelessControlls.gameplay.Speak.performed -= SpeakPerformed;
            _statelessControlls.gameplay.Disable();
        }
        
        private void SpeakPerformed(InputAction.CallbackContext ctx)
        {
            Speak?.Invoke();
        }
    }
}