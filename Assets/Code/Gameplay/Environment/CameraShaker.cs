using System;
using Code.StaticData;
using DG.Tweening;
using UnityEngine;

namespace Code.Gameplay.Environment
{
    public class CameraShaker : IDisposable
    {
        private readonly CameraConfig _config;
        private readonly Transform _transform;

        public CameraShaker(CameraConfig cameraConfig)
        {
            _config = cameraConfig;
            _transform = Camera.main.transform;
        }

        public void Dispose() => 
            _transform.DOKill();

        public void LightShake() => 
            Shake(_config.lightStrength);

        public void HardShake() => 
            Shake(_config.hardStrength);

        public void Shake(float strength)
        {
            _transform.DORewind();
            _transform.DOShakePosition(_config.duration, strength, vibrato: _config.vibrato);
        }
    }
}