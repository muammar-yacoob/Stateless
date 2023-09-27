using System;
using SparkCore.Runtime.Injection;
using UnityEngine;

namespace Player
{
    [RuntimeObject(RuntimeObjectType.Singleton)]
    public class PlayerInput : IPlayerInput, IDisposable
    {
        private GameControls controls;
        public Vector2 Move => controls.gameplay.move.ReadValue<Vector2>();
        
        public event Action Jump;
        public event Action Join;
        
        public PlayerInput()
        {
            controls = new GameControls();
            controls.gameplay.Enable();
            
            controls.gameplay.Jump.performed += ctx => Jump?.Invoke();
            //_ = _statelessControlls.gameplay.Jump.interactions.Insert(0, "Press"); // Insert Press interaction at the beginning of the list to avoid the Hold interaction
            
            controls.gameplay.Join.performed += ctx => Join?.Invoke();
        }
        public void Dispose() => controls.gameplay.Disable();
    }
}