using System;
using Cinemachine;
using Code.Core.Services.StaticData;
using Code.Gameplay.CameraManagement;
using Code.StaticData;
using UnityEngine;
using VContainer.Unity;

namespace Code.Core.Services.Camera
{
    public class CameraShakeService : IInitializable, ITickable
    {
        private readonly CameraProvider _cameraProvider;
        private readonly ConfigService _configService;

        private CinemachineBasicMultiChannelPerlin _cinemachinePerlin;
        private CameraShakeConfig _shakeConfig;

        private bool _isInitialized;
        private float _shakeTimer;
        private float _shakeTimerTotal;
        private float _startingAmplitude;
        private float _startingFrequency; // new

        public CameraShakeService(CameraProvider cameraProvider, ConfigService configService)
        {
            _cameraProvider = cameraProvider;
            _configService = configService;
        }

        public void Initialize()
        {
            _cinemachinePerlin = _cameraProvider.Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _shakeConfig = _configService.GetCameraShake();
            
            _isInitialized = true;
        }

        public void Tick()
        {
            if (_shakeTimer > 0)
            {
                _shakeTimer -= Time.deltaTime;

                _cinemachinePerlin.m_AmplitudeGain = Mathf.Lerp(_startingAmplitude, 0f, 1 - (_shakeTimer / _shakeTimerTotal));
                _cinemachinePerlin.m_FrequencyGain = Mathf.Lerp(_startingFrequency, 0f, 1 - (_shakeTimer / _shakeTimerTotal)); // new

                if (_shakeTimer <= 0)
                    StopShaking();
            }
        }

        public void LightShake() => 
            Shake(_shakeConfig.lightShakeData.amplitude, _shakeConfig.lightShakeData.frequency, _shakeConfig.lightShakeData.duration);

        public void HardShake() => 
            Shake(_shakeConfig.hardShakeData.amplitude, _shakeConfig.hardShakeData.frequency, _shakeConfig.hardShakeData.duration);

        public void Shake(float amplitude, float frequency, float duration)
        {
            if (!_isInitialized)
                throw new NullReferenceException("[Camera Shake Service] Can't shake camera, because service not initialized.");
            
            if (amplitude >= _cinemachinePerlin.m_AmplitudeGain)
            {
                _startingAmplitude = amplitude;
                _cinemachinePerlin.m_AmplitudeGain = amplitude;
            }

            if (frequency >= _cinemachinePerlin.m_FrequencyGain)
            {
                _startingFrequency = frequency; // new
                _cinemachinePerlin.m_FrequencyGain = frequency;
            }

            if (duration >= _shakeTimerTotal)
            {
                _shakeTimer = duration;
                _shakeTimerTotal = duration;
            }
        }

        public void StopShaking()
        {
            _startingAmplitude = 0f;
            _cinemachinePerlin.m_AmplitudeGain = 0f;

            _startingFrequency = 0f; // new
            _cinemachinePerlin.m_FrequencyGain = 0f;

            _shakeTimer = 0f;
            _shakeTimerTotal = 0f;
        }
    }
}