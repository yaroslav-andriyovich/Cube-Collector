using Code.Gameplay.Tracks;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    [CustomEditor(typeof(CubeSpawnPoint))]
    public class CubeSpawnPointEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected | GizmoType.Selected)]
        public static void RenderCustomGizmo(CubeSpawnPoint spawnPoint, GizmoType gizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(spawnPoint.transform.position, new Vector3(1f, 1f, 1f));
        }
    }
}