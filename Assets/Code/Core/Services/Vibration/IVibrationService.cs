namespace Code.Core.Services.Vibration
{
    public interface IVibrationService
    {
        void EnableVibration();
        void DisableVibration();
        void Stop();
        void Vibrate(int time);
    }
}