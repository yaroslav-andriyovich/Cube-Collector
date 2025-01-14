using System;
using UnityEngine;

namespace Code.Gameplay.Cubes
{
    public class WallTrigger : MonoBehaviour
    {
        [SerializeField] private string _wallTag;
        
        public event Action<Collider> Collision;
        
        private void OnTriggerEnter(Collider target)
        {
            if (HasWallTag(target))
                Collision?.Invoke(target);
        }
        
        private bool HasWallTag(Collider target) => 
            target.CompareTag(_wallTag);
    }
}