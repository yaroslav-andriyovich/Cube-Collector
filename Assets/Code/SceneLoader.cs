using UnityEngine.SceneManagement;

namespace Code
{
    public class SceneLoader
    {
        public void RestartCurrent() => 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}