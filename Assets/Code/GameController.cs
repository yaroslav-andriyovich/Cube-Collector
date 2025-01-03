using Code.Infrastructure.Services.InputService;
using Code.Player;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using VContainer;

namespace Code
{
    public class GameController : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _timeScale;
        [Space]
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private DangerousCollisionDetector _stickmanCollisionsDetector;
        //[SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private ParticleSystem _warpEffect;
        //[SerializeField] private RagdollActivator _ragdollActivator;
        //[SerializeField] private UI _ui;

        private InputService _inputService;
        private bool _gameFailed;

        private void OnDestroy()
        {
            _inputService.Actions.FirstTouch.performed -= OnFirstTouch;
            _stickmanCollisionsDetector.DangerousCollision -= StopGame;
            //_ui.RestartButton -= Restart;

            //Vibration.CancelAndroid();
        }

        [Inject]
        private void Construct(InputService inputService)
        {
            _inputService = inputService;
            
            _inputService.Actions.FirstTouch.performed += OnFirstTouch;
            
            Time.timeScale = _timeScale;
            //Vibration.Init();
            
            _stickmanCollisionsDetector.DangerousCollision += StopGame;
            //_ui.RestartButton += Restart;
        }
        
        public void Restart() => 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        private void OnFirstTouch(InputAction.CallbackContext ctx) => 
            StartGame();

        private void StartGame()
        {
            _inputService.Actions.FirstTouch.performed -= OnFirstTouch;
            _movement.enabled = true;
            //_warpEffect.Play();
            //_ui.HideStartWindow();
        }

        public void StopGame()
        {
            Restart();
            return;
            
            if (_gameFailed)
                return;
            
            _gameFailed = true;
            _movement.enabled = false;
            _inputService.DisableInput();
            //_ragdollActivator.Activate();
            //_cameraShaker.HardShake();
            //_ui.ShowRestartWindow();
        }
    }
}