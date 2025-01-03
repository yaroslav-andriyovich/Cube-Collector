using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Player
{
    public class DangerousCollisionTrigger : MonoBehaviour
    {
        [SerializeField] private List<string> _tags;
        
        public event Action DangerousCollision;
        
        private void OnCollisionEnter(Collision collision) => 
            FindTargetTags(collision);

        private void FindTargetTags(Collision collision)
        {
            foreach (string targetTag in _tags)
            {
                if (collision.gameObject.CompareTag(targetTag))
                {
                    DangerousCollision?.Invoke();
                    return;
                }
            }
        }
    }
}