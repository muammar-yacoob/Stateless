using SparkCore.Runtime.Core;
using Stateless.Candy.Events;
using Stateless.Player;
using UnityEngine;

namespace Stateless.Candy
{
    [RequireComponent(typeof(BoxCollider))]
    public class WorldCandy : InjectableMonoBehaviour
    {
        [SerializeField] int candyValue = 10;
        protected override void Awake()
        {
            base.Awake();
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                int playerIndex = playerMovement.PlayerIndex;
                //Debug.Log($"Candy going to player {playerIndex}");
                PublishEvent(new CandyCollected(transform, playerIndex, candyValue));
                //Destroy(gameObject);
            }
        }
    }
}