using System;
using TMPro;
using UnityEngine.UI;

    namespace GameEvents
    {
        public class UIEvents
        {
            public static UIEvents Instance { get; } = new UIEvents();
            public event Action<int, Image, TMP_Text> PlayerHealthUICreated;
            public void RegisterHealthBar(int playerIndex, Image healthBar, TMP_Text healthText) => PlayerHealthUICreated?.Invoke(playerIndex, healthBar, healthText);
        }
    }