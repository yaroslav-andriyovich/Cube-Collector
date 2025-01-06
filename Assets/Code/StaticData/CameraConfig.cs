using UnityEngine;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "StaticData/Camera")]
    public class CameraConfig : ScriptableObject
    {
        [Header("Shake")] 
        public float duration;
        public float lightStrength;
        public float hardStrength;
        public int vibrato;
    }
}