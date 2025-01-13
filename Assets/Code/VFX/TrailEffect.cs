using UnityEngine;

namespace Code.VFX
{
    public class TrailEffect : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trail;

        public void Enable() => 
            _trail.emitting = true;
        
        public void Disable() => 
            _trail.emitting = false;
    }
}