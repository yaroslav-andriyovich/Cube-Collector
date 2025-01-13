using Code.Core.Services.Camera;
using Code.Core.Services.Vibration;
using Code.Gameplay;
using Code.VFX;
using UnityEngine;
using VContainer;

namespace Code.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovementController _movement;
        [SerializeField] private CubeHolder _cubeHolder;
        [SerializeField, Min(0)] private int _vibrationMilliseconds;
        [SerializeField] private CharacterJumpController _characterJump;
        [SerializeField] private DangerousCollisionTrigger _dangerousCollisionTrigger;
        [SerializeField] private CharacterRagdollController _characterRagdollController;
        [SerializeField] private TrailEffect _trailEffect;

        private bool _isDead;
        private IGameControl _gameControl;
        private IVibrationService _vibrationService;
        private CameraShakeService _cameraShakeService;

        private void Awake()
        {
            _dangerousCollisionTrigger.Collided += Die;
            _cubeHolder.Emptied += Die;
            _cubeHolder.NewPlayerPosition += _characterJump.RaiseAndJump;
            _cubeHolder.CubeCollidedWithWall += OnCubeCollidedWithWall;
        }

        private void OnDestroy()
        {
            _dangerousCollisionTrigger.Collided -= Die;
            _cubeHolder.Emptied -= Die;
            _cubeHolder.NewPlayerPosition -= _characterJump.RaiseAndJump;
            _cubeHolder.CubeCollidedWithWall -= OnCubeCollidedWithWall;
            
            _gameControl.GameStarted -= OnGameStarted;
            _gameControl.GameEnded -= OnGameEnded;
        }

        [Inject]
        private void Construct(IGameControl gameControl, IVibrationService vibrationService, CameraShakeService cameraShakeService)
        {
            _gameControl = gameControl;
            _vibrationService = vibrationService;
            _cameraShakeService = cameraShakeService;
            
            _gameControl.GameStarted += OnGameStarted;
        }

        private void Die()
        {
            if (_isDead)
                return;
            
            _movement.Disable();
            _cubeHolder.ReleaseAll();
            _characterRagdollController.EnableRagdoll();
            _cameraShakeService.HardShake();
            _trailEffect.Disable();
            
            _gameControl.EndGame();
        }

        private void OnGameStarted()
        {
            _gameControl.GameStarted -= OnGameStarted;
            _gameControl.GameEnded += OnGameEnded;

            _movement.Enable();
            _trailEffect.Enable();
        }
        
        private void OnGameEnded()
        {
            _gameControl.GameEnded -= OnGameEnded;
            
            _movement.Disable();
        }

        private void OnCubeCollidedWithWall()
        {
            _cameraShakeService.LightShake();
            _vibrationService.Vibrate(_vibrationMilliseconds);
        }
    }
}