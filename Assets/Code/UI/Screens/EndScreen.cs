using Code.Gameplay;
using Code.UI.Screens.Base;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace Code.UI.Screens
{
    public class EndScreen : BaseScreen
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, Min(0f)] private float _showingDuration;
        
        private const float MAX_ALPHA = 1f;
        
        private IGameControl _gameControl;

        [Inject]
        private void Construct(IGameControl gameControl) => 
            _gameControl = gameControl;

        public override void Show()
        {
            base.Show();
            
            _canvasGroup
                .DOFade(MAX_ALPHA, _showingDuration)
                .SetLink(gameObject);
        }

        public void OnRestartButton() => 
            _gameControl.RestartGame();
    }
}