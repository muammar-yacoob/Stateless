using System.Threading;
using Cysharp.Threading.Tasks;

namespace House
{
    public interface IHouse
    {
        string HouseName { get; }
        UniTask EnterHouseAsync(CancellationToken token, int playerIndex);
        UniTask UpdateHouseAsync(CancellationToken token);
        UniTask ExitHouseAsync(CancellationToken token);
        void ProceedToNextStep();
    }
}