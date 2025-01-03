using UnityEngine;

namespace Code.Environment
{
    public class PlayerFollowingEnvironment : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;

        private void LateUpdate() => 
            MovePositionForward();

        private void MovePositionForward() => 
            transform.position = new Vector3(0f, 0f, _playerTransform.position.z);
    }
}