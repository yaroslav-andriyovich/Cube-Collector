using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.UI
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] private GameObject _startWindow;
        [SerializeField] private RectTransform _swipeImage;
        [SerializeField] private float _swipeImageSlideDistance;
        [SerializeField, Min(0f)] private float _swipeImageAnimationDuration;
        [Space]
        [SerializeField] private GameObject _restartWindow;
        [SerializeField] private CanvasGroup _restartWindowCanvasGroup;
        [SerializeField, Min(0f)] private float _restartWindowAnimationDuration;
        [SerializeField] private Button _restartButton;

        public event UnityAction RestartButtonClick
        {
            add => _restartButton.onClick.AddListener(value);
            remove => _restartButton.onClick.RemoveListener(value);
        }
        
        private const float MAX_ALPHA = 1f;

        private void OnEnable() => 
            AnimateSwipeImage();

        public void HideStartWindow() => 
            _startWindow.SetActive(false);

        public void ShowRestartWindow()
        {
            _restartWindow.SetActive(true);
            
            _restartWindowCanvasGroup.DOFade(MAX_ALPHA, _restartWindowAnimationDuration);
        }

        private void AnimateSwipeImage()
        {
            float screenWidth = Screen.width;
            float absoluteDistance = _swipeImageSlideDistance * screenWidth;

            _swipeImage
                .DOAnchorPosX(absoluteDistance, _swipeImageAnimationDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutBack)
                .SetLink(_startWindow, LinkBehaviour.KillOnDisable);
        }
    }
}