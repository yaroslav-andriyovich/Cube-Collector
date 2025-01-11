using Code.Core.SceneManagement;
using Code.Core.Services.StaticData;
using DG.Tweening;
using UnityEngine;
using VContainer.Unity;

namespace Code.Core
{
    public class GameBoot : IInitializable
    {
        private readonly StaticDataService _staticDataService;
        private readonly SceneLoader _sceneLoader;

        public GameBoot(StaticDataService staticDataService, SceneLoader sceneLoader)
        {
            Application.targetFrameRate = 60;
            DOTween.Init();

            _staticDataService = staticDataService;
            _sceneLoader = sceneLoader;
        }
        
        public void Initialize()
        {
            _staticDataService.LoadAll();
            _sceneLoader.Load(SceneNames.Main);
        }
    }
}