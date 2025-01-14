using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Core.SceneManagement
{
    public class SceneLoader
    {
        private const float PROGRESS_TO_ALLOW_SCENE_ACTIVATION = 0.9f;
        private const float DELAY_BEFORE_SCENE_ACTIVATION = 0.5f;

        public async void Load(string sceneName, Action onLoaded = null) => 
            await LoadAsync(sceneName, onLoaded);

        public async UniTask LoadAsync(string sceneName, Action onLoaded = null)
        {
            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);

            loadSceneOperation.allowSceneActivation = false;

            while (loadSceneOperation.progress < PROGRESS_TO_ALLOW_SCENE_ACTIVATION)
                await UniTask.Yield();

            await UniTask.Delay(TimeSpan.FromSeconds(DELAY_BEFORE_SCENE_ACTIVATION));
            
            loadSceneOperation.allowSceneActivation = true;

            await loadSceneOperation;

            onLoaded?.Invoke();
        }

        public void ReloadCurrent() => 
            Load(SceneManager.GetActiveScene().name);
    }
}