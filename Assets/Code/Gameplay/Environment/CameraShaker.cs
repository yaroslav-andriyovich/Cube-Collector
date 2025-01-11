using System;
using Code.Core.Services.StaticData;
using Code.StaticData;
using DG.Tweening;
using UnityEngine;
using VContainer.Unity;

namespace Code.Gameplay.Environment
{
    public class CameraShaker : IInitializable, IDisposable
    {
        private readonly StaticDataService _staticDataService;
        private readonly Transform _transform;

        private CameraConfig _config;

        public CameraShaker(StaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _transform = Camera.main.transform;
        }

        public void Initialize() => 
            _config = _staticDataService.ForCamera();

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