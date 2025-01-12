using DG.Tweening;
using UnityEngine;

namespace Code.Player
{
    public class CharacterJumpController : MonoBehaviour
    {
        [SerializeField] private Transform _characterTransform;
        [SerializeField] private Animator _animator;
        [Space]
        [SerializeField, Min(0f)] private float _jumpPower;
        [SerializeField, Min(0f)] private float _jumpDuration;
        
        private const int TWEEN_JUMPS_NUM = 0;
        private readonly int _jumpTriggerHash = Animator.StringToHash("Jump");
        
        private Tween _jumpTween;

        public void RaiseAndJump(Vector3 position)
        {
            RaiseCharacter(position);
            PlayJumpAnimation();
        }
        
        private void RaiseCharacter(Vector3 position)
        {
            CompleteJumpAnimation();
            
            _characterTransform.localPosition = position;
        }

        private void CompleteJumpAnimation() => 
            _jumpTween.Complete();
        
        private void PlayJumpAnimation()
        {
            _animator.SetTrigger(_jumpTriggerHash);

            _jumpTween = _characterTransform
                .DOLocalJump(Vector3.zero, _jumpPower, TWEEN_JUMPS_NUM, _jumpDuration)
                .SetLink(_characterTransform.gameObject)
                .SetRelative();
        }
    }
}