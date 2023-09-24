using System.Threading;
using Cysharp.Threading.Tasks;
using SparkCore.Runtime.Injection;
using StageSystem;
using UnityEngine;
using VContainer;

namespace Stages
{
    public class GreenStage : BaseStage
    {
        public override async UniTask EnterStageAsync(CancellationToken token)
        {
            Debug.Log($"Entered {this.GetType().Name} Stage");
            await base.EnterStageAsync(token);
        }

        public override async UniTask UpdateStageAsync(CancellationToken token)
        {
            foreach (var obj in gameObjectsToActivate)
            {
                obj.SetActive(true);
                await UniTask.Delay(1000, cancellationToken: token); 
            }
        }

        public override async UniTask ExitStageAsync(CancellationToken token)
        {
            Debug.Log($"Exited {this.GetType().Name} Stage");
            await base.ExitStageAsync(token);
        }
    }
}