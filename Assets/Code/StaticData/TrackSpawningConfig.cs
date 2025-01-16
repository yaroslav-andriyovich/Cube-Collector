using Code.Gameplay.Cubes;
using Code.Gameplay.Tracks;
using DG.Tweening;
using UnityEngine;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "TrackGenerationConfig", menuName = "StaticData/Track Spawning")]
    public class TrackSpawningConfig : ScriptableObject
    {
        [Header("Initial track")]
        public Track initialTrackPrefab;
        public Vector3 initialTrackPosition;
        public Quaternion initialTrackRotation;
        
        [Header("Track variants")]
        public Cube cubePrefab;
        public Track[] trackVariants;
        [Min(0)] public int initialCount;
        
        [Header("Spawn/DeSpawn")]
        [Min(0)] public int preparedCubesNumber;
        [Min(0f)] public float deSpawnDelay;

        [Header("Spawn Animation")]
        [Min(0f)] public float downwardPositionShift;
        [Min(0f)] public float forwardPositionShift;
        [Min(0f)] public float animationDuration;
        public Ease animationEase;
    }
}