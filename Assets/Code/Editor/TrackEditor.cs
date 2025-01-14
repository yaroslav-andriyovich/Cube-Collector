using Code.Gameplay.Tracks;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    [CustomEditor(typeof(Track))]
    public class TrackEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Track track = (Track)target;

            if (GUILayout.Button("Collect cube spawn points"))
            {
                track.SetCubeSpawnPoints(track.GetComponentsInChildren<CubeSpawnPoint>());
                
                EditorUtility.SetDirty(target);
            }
        }
    }
}