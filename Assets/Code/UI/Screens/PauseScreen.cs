using Code.Core.Services.Pause;
using Code.UI.Screens.Base;
using VContainer;

namespace Code.UI.Screens
{
    public class PauseScreen : BaseScreen
    {
        private IPauseService _pauseService;

        [Inject]
        private void Construct(IPauseService pauseService) => 
            _pauseService = pauseService;

        public void OnContinueButton() => 
            _pauseService.UnPause();
    }
}