using DG.Tweening;
using UnityEngine;

namespace Code.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CubeHolder _cubeHolder;
        [SerializeField] private Transform _stickmanTransform;
        [SerializeField] private Animator _animator;
        [SerializeField, Min(0f)] private float _jumpPower;
        [SerializeField, Min(0f)] private float _jumpDuration;

        private static readonly int JumpTrigger = Animator.StringToHash("Jump");
        private const int NumJumps = 0;

        private Tween _jumpTween;

        private void Awake() => 
            _cubeHolder.NewPlayerPosition += OnNewPlayerPosition;

        private void OnDestroy()
        {
            _cubeHolder.NewPlayerPosition -= OnNewPlayerPosition;
            _jumpTween?.Kill();
        }

        private void OnNewPlayerPosition(Vector3 position)
        {
            RaiseModel(position);
            PlayJumpAnimation();
        }

        private void RaiseModel(Vector3 position)
        {
            CompleteJumpAnimation();
            
            _stickmanTransform.localPosition = position;
        }

        private void CompleteJumpAnimation() => 
            _jumpTween.Complete();

        private void PlayJumpAnimation()
        {
            _animator.SetTrigger(JumpTrigger);

            _jumpTween = _stickmanTransform
                .DOLocalJump(Vector3.zero, _jumpPower, NumJumps, _jumpDuration)
                .SetRelative();
        }
    }
}