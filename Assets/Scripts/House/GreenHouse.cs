using UnityEngine.InputSystem;

namespace Stateless.House
{
    public class GreenHouse : BaseHouse
    {
        private void Update()
        {
            if (Keyboard.current.nKey.wasPressedThisFrame)
            {
                ProceedToNextStep();
            }
        }
    }
}