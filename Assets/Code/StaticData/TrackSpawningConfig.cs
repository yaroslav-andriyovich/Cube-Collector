using DG.Tweening;
using UnityEngine;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "TrackGenerationConfig", menuName = "StaticData/Track Spawning")]
    public class TrackSpawningConfig : ScriptableObject
    {
        [Header("Initial")]
        //public GameObject playerTrackInstance;
        public GameObject[] trackVariants;
        [Min(0)] public int initialCount;

        [Header("Spawning")]
        [Min(0f)] public float downwardPositionShift;
        [Min(0f)] public float forwardPositionShift;
        [Min(0f)] public float animationDuration;
        public Ease animationEase;
    }
}