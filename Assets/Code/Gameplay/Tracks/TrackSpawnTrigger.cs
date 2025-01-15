using System;
using UnityEngine;

namespace Code.Gameplay.Tracks
{
    public class TrackSpawnTrigger : MonoBehaviour
    {
        public event Action Triggered;
        
        private const string PlayerTag = "Player";
        
        private bool _isTriggered;

        private void OnEnable() => 
            _isTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!_isTriggered && IsPlayer(other))
            {
                _isTriggered = true;
                Triggered?.Invoke();
            }
        }

        private bool IsPlayer(Collider targetCollider) => 
            targetCollider.CompareTag(PlayerTag);
    }
}