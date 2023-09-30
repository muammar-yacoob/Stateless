using System.Threading;
using Cysharp.Threading.Tasks;
using StageSystem;

namespace House
{
    public class House : BaseHouse
    {
        public override async UniTask EnterHouseAsync(CancellationToken token)
        {
            await base.EnterHouseAsync(token);
        }
    }
}