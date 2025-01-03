using DG.Tweening;
using UnityEngine;

namespace Code.Environment
{
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _lightStrength;
        [SerializeField] private float _hardStrength;
        [SerializeField] private int _vibrato;

        private void OnDisable() => 
            transform.DOKill();

        public void LightShake() => 
            Shake(_lightStrength);
        
        public void HardShake() => 
            Shake(_hardStrength);

        private void Shake(float strength)
        {
            transform.DORewind();
            transform.DOShakePosition(_duration, strength, vibrato: _vibrato);
        }
    }
}