using System.Collections.Generic;
using UnityEngine;

namespace Maji.WayPointSystem
{
    public class WaypointSystem : MonoBehaviour
    {
        [SerializeField] private List<Transform> waypoints;

        private void OnDrawGizmos()
        {
            if (waypoints.Count == 0) return;

            Gizmos.color = Color.green;
            for (int i = 0; i < waypoints.Count; i++)
            {
                Gizmos.DrawSphere(waypoints[i].position, 0.01f);
                if (i > 0) 
                {
                    Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
                }
            }
        }

        public Transform GetWaypoint(int index) => waypoints[index];
        public int GetWaypointsCount() => waypoints.Count;
    }
}