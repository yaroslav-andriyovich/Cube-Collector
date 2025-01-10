using UnityEngine;

namespace Code.VFX
{
    public class TrailEffect : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trail;

        public void EnableEmitting() => 
            _trail.emitting = true;
        
        public void DisableEmitting() => 
            _trail.emitting = false;
    }
}