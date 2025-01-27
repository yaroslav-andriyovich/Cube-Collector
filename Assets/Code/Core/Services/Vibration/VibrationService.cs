using System;

namespace Code.Core.Services.Vibration
{
    public class VibrationService : IVibrationService, IDisposable
    {
        private bool _enabled = true;
        private bool _initialized;
        
        public void Initialize()
        {
            if (_initialized)
                return;
            
            global::Vibration.Init();
            _initialized = true;
        }

        public void Dispose() => 
            Stop();

        public void EnableVibration()
        {
            if (!_initialized)
                return;
            
            _enabled = true;
        }

        public void DisableVibration() => 
            _enabled = false;

        public void Stop()
        {
            if (!_initialized)
                return;
            
            global::Vibration.CancelAndroid();
        }

        public void Vibrate(int time)
        {
            if (!_initialized || !_enabled)
                return;
            
            global::Vibration.VibrateAndroid(Convert.ToInt64(time));
        }
    }
}