using System;
using Code.Infrastructure.Pools.Poolable;
using DG.Tweening;
using UnityEngine;

namespace Code.VFX
{
    public class CubeCollectionText : PoolableBase<CubeCollectionText>
    {
        [Header("Movement")]
        [SerializeField, Min(0f)] private float _jumpHeight;
        [SerializeField, Min(0f)] private float _jumpDuration;
        [SerializeField] private Ease _jumpEase;
        
        [Header("Transparency")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, Range(0f, 1f)] private float _startTransparencyValue;
        [SerializeField, Range(0f, 1f)] private float _endTransparencyValue;
        [SerializeField, Min(0f)] private float _transparencyDuration;
        [SerializeField] private Ease _transparencyEase;

        private void OnEnable()
        {
            ResetCanvasAlpha();

            OnSpawn();
        }

        private void OnDisable()
        {
            transform.DOKill();
            _canvasGroup.DOKill();
        }

        public void OnSpawn() => 
            PlayJump();

        private void ResetCanvasAlpha() => 
            _canvasGroup.alpha = _startTransparencyValue;

        private void PlayJump() =>
            transform
                .DOMoveY(_jumpHeight, _jumpDuration)
                .SetRelative()
                .SetEase(_jumpEase)
                .OnComplete(PlayInvisible);

        private void PlayInvisible() =>
            _canvasGroup
                .DOFade(_endTransparencyValue, _transparencyDuration)
                .SetEase(_transparencyEase)
                .OnComplete(Release);
    }
}