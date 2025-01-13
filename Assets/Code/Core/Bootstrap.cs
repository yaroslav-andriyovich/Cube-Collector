using Code.Core.Services.StaticData;
using Code.Core.Services.Vibration;
using DG.Tweening;
using UnityEngine;
using VContainer.Unity;

namespace Code.Core
{
    public class Bootstrap : IInitializable
    {
        private const int TARGET_FRAME_RATE = 60;
        
        private readonly ConfigService _configService;
        private readonly IVibrationService _vibrationService;

        public Bootstrap(ConfigService configService, IVibrationService vibrationService)
        {
            _configService = configService;
            _vibrationService = vibrationService;
        }
        
        public void Initialize()
        {
            Application.targetFrameRate = TARGET_FRAME_RATE;
            DOTween.Init();

            _configService.LoadAll();
            _vibrationService.Initialize();
        }
    }
}