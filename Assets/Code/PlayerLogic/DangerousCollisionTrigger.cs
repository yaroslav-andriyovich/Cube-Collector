using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.PlayerLogic
{
    public class DangerousCollisionTrigger : MonoBehaviour
    {
        [SerializeField] private List<string> _tags;
        
        public event Action Collided;
        
        private void OnCollisionEnter(Collision collision)
        {
            foreach (string dangerousTag in _tags)
            {
                if (HasTag(collision, dangerousTag))
                {
                    Collided?.Invoke();
                    return;
                }
            }
        }

        private bool HasTag(Collision collision, string tagName) => 
            collision.gameObject.CompareTag(tagName);
    }
}