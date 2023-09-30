using System.Threading;
using Cysharp.Threading.Tasks;
using StageSystem;
using UnityEngine;

namespace Stages
{
    public class RedHouse : BaseHouse
    {
        public override async UniTask EnterHouseAsync(CancellationToken token)
        {
            Debug.Log($"Entered {this.GetType().Name} Stage");
            await base.EnterHouseAsync(token);
        }

        public override async UniTask UpdateHouseAsync(CancellationToken token)
        {
            // foreach (var obj in gameObjectsToActivate)
            // {
            //     obj.SetActive(true);
            //     await UniTask.Delay(1000, cancellationToken: token); 
            // }
        }

        public override async UniTask ExitHouseAsync(CancellationToken token)
        {
            Debug.Log($"Exited {this.GetType().Name} Stage");
            await base.ExitHouseAsync(token);
        }
    }
}