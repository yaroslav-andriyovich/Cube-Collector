using Code.Gameplay;
using Code.UI.Screens.Base;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace Code.UI.Screens
{
    public class StartScreen : BaseScreen
    {
        [SerializeField] private TouchZone _touchZone;
        [SerializeField] private RectTransform _swipeImage;
        [SerializeField] private float _swipeImageSlideDistance;
        [SerializeField, Min(0f)] private float _swipeImageAnimationDuration;
        
        private IGameControl _gameControl;

        private void Awake() => 
            _touchZone.Touch += OnStartButton;

        private void OnDestroy() => 
            _touchZone.Touch -= OnStartButton;

        private void OnEnable() => 
            AnimateSwipeImage();

        [Inject]
        private void Construct(IGameControl gameControl) => 
            _gameControl = gameControl;

        public void OnStartButton() => 
            _gameControl.StartGame();
        
        private void AnimateSwipeImage()
        {
            float screenWidth = Screen.width;
            float absoluteDistance = _swipeImageSlideDistance * screenWidth;

            _swipeImage
                .DOAnchorPosX(absoluteDistance, _swipeImageAnimationDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutBack)
                .SetLink(gameObject, LinkBehaviour.KillOnDisable);
        }
    }
}