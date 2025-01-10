using System;
using Code.Gameplay.Services.GameControl;
using Code.VFX;
using UnityEngine;
using VContainer;

namespace Code.Gameplay.Environment
{
    public class PlayerFollowingEnvironment : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private FollowerData[] _followers;
        
        private CameraShaker _cameraShaker;
        private IGameEventSender _gameEventSender;

        private void LateUpdate() => 
            ChangeFollowersPosition();

        [Inject]
        private void Construct(CameraShaker cameraShaker, WarpEffect warpEffect, IGameEventSender gameEventSender)
        {
            _cameraShaker = cameraShaker;
            _gameEventSender = gameEventSender;
            
            _gameEventSender.GameEnded += _cameraShaker.HardShake; 

            _followers[1].transform = warpEffect.transform;
        }

        private void OnDestroy()
        {
            _gameEventSender.GameEnded -= _cameraShaker.HardShake;
            
            _followers[1].transform = null;
        }

        private void ChangeFollowersPosition()
        {
            foreach (FollowerData follower in _followers)
            {
                Vector3 position = follower.offset;

                position.z += _playerTransform.position.z;

                follower.transform.position = position;
                follower.transform.eulerAngles = follower.rotation;
            }
        }

        [Serializable]
        private class FollowerData
        {
            public Transform transform;
            public Vector3 offset;
            public Vector3 rotation;
        }
    }
}