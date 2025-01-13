using System;
using UnityEngine;

namespace Code.Gameplay.Cubes
{
    public class PickableWallTrigger : MonoBehaviour
    {
        [SerializeField] private string _wallTag;
        
        public event Action<Collider> WallCollision;
        
        private void OnTriggerEnter(Collider target)
        {
            if (HasWallTag(target))
                WallCollision?.Invoke(target);
        }
        
        private bool HasWallTag(Collider target) => 
            target.CompareTag(_wallTag);
    }
}