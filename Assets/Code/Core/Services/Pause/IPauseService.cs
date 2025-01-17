using System;

namespace Code.Core.Services.Pause
{
    public interface IPauseService
    {
        public event Action Paused;
        public event Action UnPaused;
        
        void Pause();
        void UnPause();
    }
}