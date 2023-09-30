using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace House
{
    public interface IHouse
    {
        string HouseName { get; }
        event Action<int, int> CandyCollected;
        event Action<IHouse> HouseEntered;
        event Action<IHouse> HouseExited;

        UniTask EnterHouseAsync(CancellationToken token, int playerIndex);
        UniTask UpdateHouseAsync(CancellationToken token);
        UniTask ExitHouseAsync(CancellationToken token);
        void ProceedToNextStep();
    }
}