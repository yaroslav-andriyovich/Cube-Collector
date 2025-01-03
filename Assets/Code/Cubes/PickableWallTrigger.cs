using System;
using UnityEngine;

namespace Code.Cubes
{
    public class PickableWallTrigger : MonoBehaviour
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