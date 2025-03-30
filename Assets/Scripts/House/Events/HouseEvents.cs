using System;
using System.Threading;
using UnityEngine.UI;
using UnityEngine;

namespace Stateless.House.Events
{
    public class HouseEvents 
    {
        public static HouseEvents Instance { get; } = new();
        
        public event Action<Sprite, string, string, CancellationToken> DialogueStarted;
        public event Action HouseEntered;
        public event Action HouseExited;
        
        public void StartDialogue (Sprite speakerSprite, string speakerName, string dialogue, CancellationToken token) => DialogueStarted?.Invoke(speakerSprite, speakerName, dialogue, token);
        public void EnterHouse() => HouseEntered?.Invoke();
        public void ExitHouse() => HouseExited?.Invoke();
    }
}