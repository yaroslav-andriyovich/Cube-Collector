using Code.Core.SceneManagement;
using Code.Core.Services.StaticData;
using DG.Tweening;
using UnityEngine;
using VContainer.Unity;

namespace Code.Core
{
    public class GameBoot : IInitializable
    {
        private const int TARGET_FRAME_RATE = 60;
        
        private readonly ConfigService _configService;
        private readonly SceneLoader _sceneLoader;

        public GameBoot(ConfigService configService, SceneLoader sceneLoader)
        {
            Application.targetFrameRate = TARGET_FRAME_RATE;
            DOTween.Init();

            _configService = configService;
            _sceneLoader = sceneLoader;
        }
        
        public void Initialize()
        {
            _configService.LoadAll();
            LoadInitialScene();
        }

        private void LoadInitialScene() => 
            _sceneLoader.Load(SceneNames.Main);
    }
}