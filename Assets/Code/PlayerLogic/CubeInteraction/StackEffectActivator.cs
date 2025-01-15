using Code.Gameplay.Cubes;
using UnityEngine;

namespace Code.PlayerLogic.CubeInteraction
{
    public class StackEffectActivator : MonoBehaviour
    {
        [SerializeField] private CubeHolder _cubeHolder;
        [SerializeField] private ParticleSystem _particle;
        
        private void Awake()
        {
            _cubeHolder.NewPlayerPosition += OnCubeCollected;
            _cubeHolder.StackOverflow += OnStackOverflow;
        }

        private void OnDestroy()
        {
            _cubeHolder.NewPlayerPosition -= OnCubeCollected;
            _cubeHolder.StackOverflow -= OnStackOverflow;
        }

        private void OnCubeCollected(Vector3 at)
        {
            Vector3 worldPosition = transform.TransformPoint(at);
            
            _particle.Stop();
            _particle.transform.position = worldPosition;
            _particle.Play();
        }

        private void OnStackOverflow(Cube cube)
        {
            _particle.Stop();
            _particle.transform.position = cube.transform.position;
            _particle.Play();
        }
    }
}