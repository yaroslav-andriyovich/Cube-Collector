using System;
using UnityEngine;

namespace Code.Core.Services.Pause
{
    public class PauseService : IPauseService
    {
        public event Action Paused;
        public event Action UnPaused;

        public void Pause()
        {
            Time.timeScale = 0;
            Paused?.Invoke();
        }

        public void UnPause()
        {
            Time.timeScale = 1;
            UnPaused?.Invoke();
        }
    }
}