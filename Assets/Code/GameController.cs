using Code.Gameplay.Environment;
using Code.Infrastructure.Services.Input;
using Code.Player;
using Code.UI;
using UnityEngine;
using VContainer;

namespace Code
{
    public class GameController : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _timeScale;
        [Space]
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private DangerousCollisionTrigger _stickmanCollisionsTrigger;
        [SerializeField] private ParticleSystem _warpEffect;
        [SerializeField] private RagdollActivator _ragdollActivator;
        [SerializeField] private UIController _ui;

        private IInputService _inputService;
        private SceneLoader _sceneLoader;
        private CameraShaker _cameraShaker;
        private bool _gameFailed;

        private void OnDestroy()
        {
            _inputService.Touch -= OnTouch;
            _stickmanCollisionsTrigger.DangerousCollision -= StopGame;
            _ui.RestartButton -= Restart;
        }

        [Inject]
        private void Construct(IInputService inputService, SceneLoader sceneLoader, CameraShaker cameraShaker)
        {
            _inputService = inputService;
            _sceneLoader = sceneLoader;
            _cameraShaker = cameraShaker;
            
            _inputService.Touch += OnTouch;
            
            Time.timeScale = _timeScale;

            _stickmanCollisionsTrigger.DangerousCollision += StopGame;
            _ui.RestartButton += Restart;
            
            _inputService.EnableInput();
        }

        public void Restart() =>
            _sceneLoader.RestartCurrent();

        private void OnTouch()
        {
            _inputService.Touch -= OnTouch;
            
            StartGame();
        }

        private void StartGame()
        {
            _movement.enabled = true;
            _warpEffect.Play();
            _ui.HideStartWindow();
        }

        public void StopGame()
        {
            if (_gameFailed)
                return;
            
            _gameFailed = true;
            _movement.enabled = false;
            _inputService.DisableInput();
            _ragdollActivator.Activate();
            _warpEffect.Stop();
            _cameraShaker.HardShake();
            _ui.ShowRestartWindow();
        }
    }
}