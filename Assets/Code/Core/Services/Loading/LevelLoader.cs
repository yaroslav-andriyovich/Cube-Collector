using UnityEngine.SceneManagement;

namespace Code.Core.Services.Loading
{
    public class LevelLoader
    {
        public void ReloadCurrent() => 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}