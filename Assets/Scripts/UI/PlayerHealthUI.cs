using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stateless.UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] Image healthBar;
        [SerializeField] TMP_Text healthText;
        [SerializeField] int playerIndex;
        
        private void Awake()
        {
            healthBar.fillAmount = 1;
            healthText.text = "100%";
            
            GameEvents.UIEvents.Instance.RegisterHealthBar(playerIndex, healthBar, healthText);
        }
    }
}
