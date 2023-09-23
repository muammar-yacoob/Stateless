using System;
using System.Threading;
using BornCore;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using WayPointSystem;

public class WaypointFollower : InjectableMonoBehaviour
{
    [SerializeField] private WaypointSystem waypointSystem;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotationSpeed = 2f;
        
    public float MoveSpeed => moveSpeed;
    public float CurrentSmoothSpeed { get; private set; }

    private Transform target;
    private int currentWaypointIndex = -1; 

    public event Action<Transform> OnTargetReached;
        
    private CancellationTokenSource cts = new ();
        
    private async void Start()
    {
#if UNITY_EDITOR
        await UniTask.Delay(500);
        Selection.activeObject = gameObject;
#endif
    }

    public void GoTo(int targetIndex)
    {
        cts?.Cancel(); 
        cts = new CancellationTokenSource(); 
        currentWaypointIndex = targetIndex;
        MoveToTarget(waypointSystem.GetWaypoint(currentWaypointIndex), cts.Token).Forget();
    }

    private async UniTask MoveToTarget(Transform waypoint, CancellationToken ct)
    {
        Tween rotationTween = transform.DOLookAt(waypoint.position, rotationSpeed, AxisConstraint.None, Vector3.up);
        //await UniTask.WaitUntil(() => !rotationTween.IsActive());//.WithCancellation(ct);

        float distance = Vector3.Distance(transform.position, waypoint.position);
        float duration = distance / moveSpeed;
        float startTime = Time.time;

        Tween moveTween = transform.DOMove(waypoint.position, duration)
            .SetEase(Ease.InOutSine)
            .OnUpdate(() => 
            {
                float timePassed = Time.time - startTime;
                float fractionOfJourney = timePassed / duration;
                CurrentSmoothSpeed = Mathf.Lerp(0, moveSpeed, fractionOfJourney);
            })
            .OnComplete(() => OnTargetReached?.Invoke(waypoint));

        await UniTask.WaitUntil(() => !moveTween.IsActive());//.WithCancellation(ct);
    }
        
    [ContextMenu("Go to next")]
    public void GoToNext() => GoTo((currentWaypointIndex + 1) % waypointSystem.GetWaypointsCount());

    [ContextMenu("Go to previous")]
    public void GoToPrevious() => GoTo((currentWaypointIndex - 1 + waypointSystem.GetWaypointsCount()) % waypointSystem.GetWaypointsCount());
}