using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StageSystem
{
    public interface IHouse
    {
        string HouseId { get; }
        event Action<int> CandyCollected;
        event Action<IHouse> HouseEntered;
        event Action<IHouse> HouseExited;

        UniTask EnterHouseAsync(CancellationToken token);
        UniTask UpdateHouseAsync(CancellationToken token);
        UniTask ExitHouseAsync(CancellationToken token);
    }

    [Serializable]
    public class StageStep
    {
        public AudioClip VoiceOver;
        public List<GameObject> GameObjectsToActivate;
        public Color TintColor = Color.white;
    }
}