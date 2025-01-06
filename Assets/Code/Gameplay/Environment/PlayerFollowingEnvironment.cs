using System;
using UnityEngine;

namespace Code.Gameplay.Environment
{
    public class PlayerFollowingEnvironment : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private FollowerData[] _followers;

        private void LateUpdate() => 
            ChangeFollowersPosition();

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