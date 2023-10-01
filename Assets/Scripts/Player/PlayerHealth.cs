using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    
    [Header("Bar Colors")]
    [SerializeField] private Color healthyColor = Color.green;
    [SerializeField] private Color injuredColor = Color.yellow;
    [SerializeField] private Color criticalColor = Color.red;
    
    private TextMeshProUGUI healthText;
    private Image healthBar;
    private float currentHealth;
    private PlayerInput playerInput;

    private void Awake()
    {
        currentHealth = maxHealth;
        GameEvents.UIEvents.Instance.PlayerHealthUICreated += SetupPlayerHealthUI;
        UpdateUI();
    }

    private void SetupPlayerHealthUI(int arg1, Image arg2, TMP_Text arg3)
    {
        //TODO: find and assign player health bar
        playerInput = GetComponent<PlayerInput>();
        //healthText = 
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
    }

    private void UpdateUI()
    {
        float fillValue = currentHealth / maxHealth;
        healthText.text = fillValue.ToString("P0");
        healthBar.DOFillAmount(fillValue, 0.2f);
        
        if (currentHealth / maxHealth <= 0.2f)
        {
            healthBar.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            healthBar.color = Color.red;
        }
        else if (currentHealth / maxHealth <= 0.5f)
        {
            healthBar.DOKill(); // Stop any existing DoTween animations
            healthBar.color = Color.yellow;
            healthBar.DOFade(1f, 0.5f); // Reset to full opacity
        }
        else
        {
            healthBar.DOKill(); // Stop any existing DoTween animations
            healthBar.color = Color.green;
            healthBar.DOFade(1f, 0.5f); // Reset to full opacity
        }
        Debug.Log($"Ouch! {currentHealth}%");
    }
}