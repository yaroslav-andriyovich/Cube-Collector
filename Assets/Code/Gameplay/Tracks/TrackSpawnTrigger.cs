using System;
using Code.Gameplay.Systems;
using UnityEngine;

namespace Code.Gameplay.Tracks
{
    public class TrackSpawnTrigger : MonoBehaviour
    {
        public event Action Triggered;
        
        private const string PlayerTag = "Player";
        
        private bool _isTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (!_isTriggered && IsPlayer(other))
            {
                //InvokeTrackGenerator();
                _isTriggered = true;
                Triggered?.Invoke();
            }
        }

        private bool IsPlayer(Collider targetCollider) => 
            targetCollider.CompareTag(PlayerTag);

        private void InvokeTrackGenerator()
        {
            TrackSpawner trackSpawner = FindObjectOfType<TrackSpawner>();

            trackSpawner.GenerateNext();
        }
    }
}