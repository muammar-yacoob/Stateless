using StageSystem;
using UnityEngine.InputSystem;

namespace Stages
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