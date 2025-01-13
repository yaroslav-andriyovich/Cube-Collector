using Code.Gameplay.Services.GameControl;
using Code.Player;
using UnityEngine;
using VContainer;

namespace Code.VFX
{
    public class WarpEffect : MonoBehaviour
    {
        [SerializeField] private Vector3 _playerOffset;
        [SerializeField] private ParticleSystem _particle;
        
        private IGameEventSender _gameEventSender;
        private Transform _playerTransform;

        [Inject]
        private void Construct(IGameEventSender gameEventSender, PlayerController playerController)
        {
            _gameEventSender = gameEventSender;
            _playerTransform = playerController.transform;

            _gameEventSender.GameStarted += _particle.Play;
            _gameEventSender.GameEnded += _particle.Stop;
        }

        private void OnDestroy()
        {
            _gameEventSender.GameStarted -= _particle.Play;
            _gameEventSender.GameEnded -= _particle.Stop;
        }
        
        private void LateUpdate() => 
            MoveToPlayer();
        
        private void MoveToPlayer()
        {
            Vector3 position = _playerOffset;

            position.z += _playerTransform.position.z;

            transform.position = position;
        }
    }
}