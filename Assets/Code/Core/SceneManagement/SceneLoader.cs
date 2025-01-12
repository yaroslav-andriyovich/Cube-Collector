using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Core.SceneManagement
{
    public class SceneLoader
    {
        private const float PROGRESS_TO_ALLOW_SCENE_ACTIVATION = 0.9f;
        private const float PROGRESS_FULL = 1f;
        private const float DELAY_BEFORE_SCENE_ACTIVATION = 1f;
        
        private readonly ILoadingScreen _loadingScreen;

        private string CurrentScene => SceneManager.GetActiveScene().name;

        public SceneLoader(ILoadingScreen loadingScreen) => 
            _loadingScreen = loadingScreen;

        public async void Load(string sceneName, Action onLoaded = null) => 
            await LoadAsync(sceneName, onLoaded);

        public async UniTask LoadAsync(string sceneName, Action onLoaded = null)
        {
            _loadingScreen.Show();

            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);

            loadSceneOperation.allowSceneActivation = false;

            while (loadSceneOperation.progress < PROGRESS_TO_ALLOW_SCENE_ACTIVATION)
            {
                float progress = loadSceneOperation.progress / PROGRESS_TO_ALLOW_SCENE_ACTIVATION;

                _loadingScreen.UpdateProgress(Mathf.Clamp01(progress));
                
                await UniTask.Yield();
            }
            
            _loadingScreen.UpdateProgress(PROGRESS_FULL);
            
            await UniTask.Delay(TimeSpan.FromSeconds(DELAY_BEFORE_SCENE_ACTIVATION));
            
            loadSceneOperation.allowSceneActivation = true;

            await loadSceneOperation;

            onLoaded?.Invoke();
            
            _loadingScreen.Hide();
        }

        public void ReloadCurrent() => 
            Load(SceneManager.GetActiveScene().name);

        public bool IsCurrentScene(string sceneName) => 
            sceneName == CurrentScene;
    }
}