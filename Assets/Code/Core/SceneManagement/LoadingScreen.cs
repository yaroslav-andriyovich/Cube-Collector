using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Core.SceneManagement
{
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, Min(0f)] private float _fadeDuration;
        [SerializeField] private Image _progressBar;

        private void Awake() => 
            DontDestroyOnLoad(this);

        public void Show()
        {
            _canvasGroup.alpha = 1;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            _canvasGroup
                .DOFade(0, _fadeDuration)
                .SetLink(gameObject)
                .OnComplete(() => gameObject.SetActive(false));
        }

        public void UpdateProgress(float progress) => 
            _progressBar.fillAmount = progress;
    }
}