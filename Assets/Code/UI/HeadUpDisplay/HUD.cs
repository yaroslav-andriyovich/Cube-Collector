using Code.Core.Services.Pause;
using TMPro;
using UnityEngine;
using VContainer;

namespace Code.UI.HeadUpDisplay
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TMP_Text _collectedCubesText;

        private IPauseService _pauseService;

        [Inject]
        private void Construct(IPauseService pauseService) => 
            _pauseService = pauseService;

        public void Show() => 
            gameObject.SetActive(true);
        
        public void Hide() => 
            gameObject.SetActive(false);

        public void OnPauseButton() => 
            _pauseService.Pause();

        public void UpdateCollectedCubesNumber(int number) => 
            _collectedCubesText.text = number.ToString();
    }
}