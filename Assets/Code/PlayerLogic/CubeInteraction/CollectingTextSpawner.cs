using Code.Core.Services.Pools;
using Code.VFX;
using UnityEngine;
using VContainer;

namespace Code.PlayerLogic.CubeInteraction
{
    public class CollectingTextSpawner : MonoBehaviour
    {
        [SerializeField] private CubeHolder _cubeHolder;
        [SerializeField] private CubeCollectionText _effectPrefab;
        [SerializeField, Min(0)] private int _countToWarmUp;
        
        private PoolService _poolService;

        private void Awake() => 
            _cubeHolder.NewPlayerPosition += OnCubeCollected;

        private void OnDestroy() => 
            _cubeHolder.NewPlayerPosition -= OnCubeCollected;

        [Inject]
        private void Construct(PoolService poolService)
        {
            _poolService = poolService;
            _poolService.Warmup(_effectPrefab, _countToWarmUp);
        }

        private void OnCubeCollected(Vector3 at)
        {
            Vector3 worldPosition = transform.TransformPoint(at);
            
            _poolService.Spawn(_effectPrefab, worldPosition, Quaternion.identity);
        }
    }
}