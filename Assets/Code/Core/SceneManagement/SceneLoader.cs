using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Core.SceneManagement
{
    public class SceneLoader
    {
        private readonly ILoadingScreen _loadingScreen;

        public SceneLoader(ILoadingScreen loadingScreen) => 
            _loadingScreen = loadingScreen;

        public async void Load(string sceneName, Action onLoaded = null) => 
            await LoadAsync(sceneName, onLoaded);

        public async UniTask LoadAsync(string sceneName, Action onLoaded = null)
        {
            _loadingScreen.Show();

            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);

            loadSceneOperation.allowSceneActivation = false;

            while (loadSceneOperation.progress < 0.9f)
            {
                float progress = loadSceneOperation.progress / 0.9f;

                _loadingScreen.UpdateProgress(Mathf.Clamp01(progress));
                
                await UniTask.Yield();
            }
            
            _loadingScreen.UpdateProgress(1f);
            
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            
            loadSceneOperation.allowSceneActivation = true;

            await loadSceneOperation;

            onLoaded?.Invoke();
            
            _loadingScreen.Hide();
        }

        public void ReloadCurrent() => 
            Load(SceneManager.GetActiveScene().name);
    }
}