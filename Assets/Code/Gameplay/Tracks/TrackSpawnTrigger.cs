using UnityEngine;

namespace Code.Gameplay.Tracks
{
    public class TrackSpawnTrigger : MonoBehaviour
    {
        private const string PlayerTag = "Player";
        
        private bool _isTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (!_isTriggered && IsPlayer(other))
            {
                InvokeTrackGenerator();
                _isTriggered = true;
            }
        }

        private bool IsPlayer(Collider targetCollider) => 
            targetCollider.CompareTag(PlayerTag);

        private void InvokeTrackGenerator()
        {
            TrackGenerator trackGenerator = FindObjectOfType<TrackGenerator>();

            trackGenerator.GenerateNext();
        }
    }
}