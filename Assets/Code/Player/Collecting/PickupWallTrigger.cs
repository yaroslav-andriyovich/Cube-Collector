using System;
using UnityEngine;

namespace Code.Player.Collecting
{
    public class PickupWallTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask _wallLayer;
        
        public event Action<Collider> WallCollision;
        
        private void OnTriggerEnter(Collider target)
        {
            if (HasWallLayer(target))
                WallCollision?.Invoke(target);
        }
        
        private bool HasWallLayer(Collider target) => 
            _wallLayer == (_wallLayer | 1 << target.gameObject.layer);
    }
}