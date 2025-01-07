using System;
using VContainer.Unity;

namespace Code.Core.Services.Vibration
{
    public class VibrationService : IVibrationService, IInitializable, IDisposable
    {
        private bool _enabled = true;
        private bool _hasVibrator;
        
        public void Initialize()
        {
            if (!CheckVibratorAvailability())
                return;
            
            global::Vibration.Init();
        }

        public void Dispose() => 
            Stop();

        public void EnableVibration()
        {
            if (!_hasVibrator)
                return;
            
            _enabled = true;
        }

        public void DisableVibration() => 
            _enabled = false;

        public void Stop()
        {
            if (!_hasVibrator)
                return;
            
            global::Vibration.CancelAndroid();
        }

        public void Vibrate(int time)
        {
            if (!_enabled)
                return;
            
            global::Vibration.VibrateAndroid(Convert.ToInt64(time));
        }

        private bool CheckVibratorAvailability()
        {
            _hasVibrator = global::Vibration.HasVibrator();

            return _hasVibrator;
        }
    }
}