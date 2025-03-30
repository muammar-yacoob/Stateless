using System.Threading;
using Cysharp.Threading.Tasks;

namespace Stateless.House
{
    public interface IHouse
    {
        UniTask EnterHouseAsync(CancellationToken token, int playerIndex);
        UniTask UpdateHouseAsync(CancellationToken token);
        UniTask ExitHouseAsync(CancellationToken token);
        void ProceedToNextStep();
    }
}