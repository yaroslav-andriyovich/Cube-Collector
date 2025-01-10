using Code.Gameplay.Services.GameControl;
using Code.VFX;
using UnityEngine;
using VContainer;

namespace Code.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovementController _movement;
        [SerializeField] private CubeHolder _cubeHolder;
        [SerializeField] private CharacterJumpController _characterJump;
        [SerializeField] private DangerousCollisionTrigger _dangerousCollisionTrigger;
        [SerializeField] private CharacterRagdollController _characterRagdollController;
        [SerializeField] private TrailEffect _trailEffect;

        private bool _isDead;
        private IGameControl _gameControl;

        private void Awake()
        {
            _dangerousCollisionTrigger.Collided += Die;
            _cubeHolder.Emptied += Die;
            _cubeHolder.NewPlayerPosition += _characterJump.RaiseAndJump;
        }

        private void OnDestroy()
        {
            _dangerousCollisionTrigger.Collided -= Die;
            _cubeHolder.Emptied -= Die;
            _cubeHolder.NewPlayerPosition -= _characterJump.RaiseAndJump;
            
            _gameControl.GameStarted -= OnGameStarted;
            _gameControl.GameEnded -= OnGameEnded;
        }

        [Inject]
        private void Construct(IGameControl gameControl)
        {
            _gameControl = gameControl;
            _gameControl.GameStarted += OnGameStarted;
        }

        private void Die()
        {
            if (_isDead)
                return;
            
            _movement.Disable();
            _characterRagdollController.EnableRagdoll();
            _trailEffect.DisableEmitting();
            
            _gameControl.EndGame();
        }

        private void OnGameStarted()
        {
            _gameControl.GameStarted -= OnGameStarted;
            _gameControl.GameEnded += OnGameEnded;

            _movement.Enable();
            _trailEffect.EnableEmitting();
        }
        
        private void OnGameEnded()
        {
            _movement.Disable();
            _gameControl.GameEnded -= OnGameEnded;
        }
    }
}