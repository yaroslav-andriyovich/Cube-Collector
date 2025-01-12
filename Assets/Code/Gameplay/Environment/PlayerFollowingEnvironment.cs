using System;
using Code.VFX;
using UnityEngine;
using VContainer;

namespace Code.Gameplay.Environment
{
    public class PlayerFollowingEnvironment : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private FollowerData[] _followers;

        private void LateUpdate() => 
            ChangeFollowersPosition();

        [Inject]
        private void Construct(WarpEffect warpEffect)
        {
            _followers[0].transform = warpEffect.transform;
        }

        private void OnDestroy() => 
            _followers[0].transform = null;

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