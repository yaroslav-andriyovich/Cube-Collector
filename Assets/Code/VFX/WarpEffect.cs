using Code.Gameplay.Services.GameControl;
using UnityEngine;
using VContainer;

namespace Code.VFX
{
    public class WarpEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle;
        
        private IGameEventSender _gameEventSender;

        [Inject]
        private void Construct(IGameEventSender gameEventSender)
        {
            _gameEventSender = gameEventSender;

            _gameEventSender.GameStarted += _particle.Play;
            _gameEventSender.GameEnded += _particle.Stop;
        }

        private void OnDestroy()
        {
            _gameEventSender.GameStarted -= _particle.Play;
            _gameEventSender.GameEnded -= _particle.Stop;
        }
    }
}