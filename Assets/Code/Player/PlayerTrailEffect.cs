using UnityEngine;

namespace Code.Player
{
    public class PlayerTrailEffect : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trail;
        [SerializeField] private DangerousCollisionTrigger _dangerousCollisionTrigger;
        
        private void Start() => 
            _dangerousCollisionTrigger.DangerousCollision += DisableEmitting;

        private void OnDestroy() => 
            _dangerousCollisionTrigger.DangerousCollision -= DisableEmitting;

        private void DisableEmitting() => 
            _trail.emitting = false;
    }
}