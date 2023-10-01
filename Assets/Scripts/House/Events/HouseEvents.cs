using System;
using System.Threading;

namespace GameEvents
{
    public class HouseEvents 
    {
        public static HouseEvents Instance { get; } = new();
        
        public event Action<string, CancellationToken> DialogueStarted;
        public event Action HouseEntered;
        public event Action HouseExited;
        
        public void StartDialogue(string dialogue, CancellationToken token) => DialogueStarted?.Invoke(dialogue, token);
        public void EnterHouse() => HouseEntered?.Invoke();
        public void ExitHouse() => HouseExited?.Invoke();
    }
}