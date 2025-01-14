using Code.Core.Services.Camera;
using Code.Core.Services.Vibration;
using Code.Gameplay;
using Code.VFX;
using UnityEngine;
using VContainer;

namespace Code.PlayerLogic
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private CubeHolder _cubeHolder;
        [SerializeField, Min(0)] private int _vibrationMilliseconds;
        [SerializeField] private CharacterJump _characterJump;
        [SerializeField] private DangerousCollisionTrigger _dangerousCollisionTrigger;
        [SerializeField] private CharacterRagdoll _characterRagdoll;
        [SerializeField] private TrailEffect _trailEffect;

        private bool _isDead;
        private IGameControl _gameControl;
        private IVibrationService _vibrationService;
        private CameraShakeService _cameraShakeService;

        private void Awake()
        {
            _dangerousCollisionTrigger.Collided += Die;
            _cubeHolder.Emptied += Die;
            _cubeHolder.NewPlayerPosition += _characterJump.Jump;
            _cubeHolder.CubeCollidedWithWall += OnCubeCollidedWithWall;
        }

        private void OnDestroy()
        {
            _dangerousCollisionTrigger.Collided -= Die;
            _cubeHolder.Emptied -= Die;
            _cubeHolder.NewPlayerPosition -= _characterJump.Jump;
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
            _characterRagdoll.Enable();
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