using UnityEngine.InputSystem;

namespace House
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