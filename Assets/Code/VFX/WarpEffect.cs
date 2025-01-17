using UnityEngine;

namespace Code.VFX
{
    public class WarpEffect : MonoBehaviour
    {
        [SerializeField] private Vector3 _playerOffset;
        [SerializeField] private ParticleSystem _particle;
        
        private Transform _playerTransform;

        private void LateUpdate()
        {
            if (_playerTransform is null)
                return;
            
            Vector3 position = _playerOffset;

            position.z += _playerTransform.position.z;

            transform.position = position;
        }

        public void SetTarget(Transform target) => 
            _playerTransform = target;

        public void Enable()
        {
            enabled = true;
            _particle.Play();
        }

        public void Disable()
        {
            enabled = false;
            _particle.Stop();
        }
    }
}