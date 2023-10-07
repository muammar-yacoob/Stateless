using UnityEngine;

namespace Stateless.Candy.Events
{
    public class CandyCollected
    {
        public readonly Transform candyTransform;
        public readonly int PlayerIndex;
        public readonly int CandyValue;

        public CandyCollected(Transform candyTransform, int playerIndex, int candyValue)
        {
            this.candyTransform = candyTransform;
            PlayerIndex = playerIndex;
            CandyValue = candyValue;
        }
    }
}