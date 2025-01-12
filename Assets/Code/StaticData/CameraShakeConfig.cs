using System;
using UnityEngine;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "StaticData/Camera")]
    public class CameraShakeConfig : ScriptableObject
    {
        [Header("Shake")]
        public CameraShakeData lightShakeData;
        public CameraShakeData hardShakeData;
    }

    [Serializable]
    public struct CameraShakeData
    {
        public float amplitude;
        public float frequency;
        public float duration;
    }
}