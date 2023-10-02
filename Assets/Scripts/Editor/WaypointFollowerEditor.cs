using Stateless.WayPointSystem;
using UnityEditor;
using UnityEngine;

namespace Stateless.Editor
{
    [CustomEditor(typeof(WaypointFollower))]
    public class WaypointFollowerEditor : UnityEditor.Editor
    {
        private int goToIndex = 0;

        private bool showWaypointControls = true;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Controls are only available in Play Mode.", MessageType.Info);
                return;
            }

            EditorGUILayout.Space();

            WaypointFollower waypointFollower = (WaypointFollower)target;

            showWaypointControls = EditorGUILayout.Foldout(showWaypointControls, "Waypoint Controls");
            if (showWaypointControls)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Next")) waypointFollower.GoToNext();
                if (GUILayout.Button("Previous")) waypointFollower.GoToPrevious();
                GUILayout.EndHorizontal();

                EditorGUILayout.Space();

                GUILayout.BeginHorizontal();
                goToIndex = EditorGUILayout.IntField("Go To Index:", goToIndex);
                if (GUILayout.Button("Go To Index")) waypointFollower.GoTo(goToIndex);
                GUILayout.EndHorizontal();
            }
        }
    }
}