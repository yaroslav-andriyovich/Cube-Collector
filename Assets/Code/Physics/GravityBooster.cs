using UnityEngine;

namespace Code.Physics
{
    public class GravityBooster : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _attraction;

        private void FixedUpdate() => 
            _rigidbody.AddForce(Vector3.down * _attraction);
    }
}