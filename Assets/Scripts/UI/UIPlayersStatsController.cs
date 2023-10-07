using SparkCore.Runtime.Core;
using UnityEngine;

namespace Stateless.UI
{
    public class UIPlayersStatsController : InjectableMonoBehaviour
    {
        [SerializeField] private RectTransform playerStatsVisuals;
        protected override void Awake()
        {
            base.Awake();
            playerStatsVisuals.gameObject.SetActive(false);
        }
        
        
    }
}