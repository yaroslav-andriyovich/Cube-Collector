using System.Collections.Generic;
using Code.Core.Pools;
using Code.Core.Services.Pools;
using Code.Gameplay.Cubes;
using Code.StaticData;
using DG.Tweening;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Gameplay.Tracks
{
    public class TrackSpawner : MonoBehaviour, IInitializable
    {
        [SerializeField] private GameObject _playerTrackInstance;
        [SerializeField] private PickableCube _pickableCubePrefab;

        private GameObject FirstSpawned => _spawned[0];
        private GameObject LastSpawned => _spawned[^1];

        private TrackSpawningConfig _config;
        private List<GameObject> _spawned;
        private List<PickableCube> _markedToDestroy;
        
        private PoolService _poolService;
        private MonoPool<PickableCube> _pickablesPool;
        private List<PickableCube> _spawnedPickables;
        private IObjectResolver _objectResolver;

        private void Awake()
        {
            Initialize();
            InstantiateInitialTracks();
        }

        [Inject]
        private void Construct(TrackSpawningConfig config, PoolService poolService, IObjectResolver objectResolver)
        {
            _config = config;
            _poolService = poolService;
            _objectResolver = objectResolver;
        }

        public void Initialize()
        {
            _spawned = new List<GameObject>
            {
                _playerTrackInstance
            };

            _spawnedPickables = new List<PickableCube>();

            _markedToDestroy = new List<PickableCube>();
            
            _pickablesPool = _poolService.CreatePool(_pickableCubePrefab);
            _pickablesPool.Warmup(20);
        }

        public void GenerateNext()
        {
            Vector3 position = GetNextPosition();
            GameObject spawnedTrack = InstantiateRandomPrefab(position);

            AnimateLift(spawnedTrack);
        }

        public void MarkUnusedPickable(PickableCube cube) =>
            _markedToDestroy.Add(cube);

        private void InstantiateInitialTracks()
        {
            for (int i = 0; i < _config.initialCount; i++)
            {
                Vector3 position = LastSpawned.transform.position;

                position = ShiftPositionForward(position);

                InstantiateRandomPrefab(position);
            }
        }

        private Vector3 ShiftPositionForward(Vector3 position)
        {
            position.z += _config.forwardPositionShift;
            return position;
        }

        private GameObject InstantiateRandomPrefab(Vector3 at)
        {
            GameObject prefab = GetRandomPrefab();
            GameObject track = _objectResolver.Instantiate(prefab, at, Quaternion.identity);

            _spawned.Add(track);
            return track;
        }

        private GameObject GetRandomPrefab()
        {
            int startIndex = 0;
            int endIndex = _config.trackVariants.Length;
            int randomIndex = Random.Range(startIndex, endIndex);
            GameObject randomTrack = _config.trackVariants[randomIndex].gameObject;

            return randomTrack;
        }

        private Vector3 GetNextPosition()
        {
            Vector3 lastTrackPosition = LastSpawned.transform.position;

            lastTrackPosition = ShiftPositionForward(lastTrackPosition);
            lastTrackPosition = ShiftPositionDown(lastTrackPosition);

            return lastTrackPosition;
        }

        private Vector3 ShiftPositionDown(Vector3 position)
        {
            position.y = -_config.downwardPositionShift;
            return position;
        }

        private void AnimateLift(GameObject track)
        {
            float targetPosition = FirstSpawned.transform.position.y;

            track.transform
                .DOMoveY(targetPosition, _config.animationDuration)
                .SetEase(_config.animationEase)
                .OnComplete(CollectGarbage);
        }

        private void CollectGarbage()
        {
            DestroyTraveledTrack();
            ReleaseUnusedPickables();
        }

        private void DestroyTraveledTrack()
        {
            GameObject traveledTrack = FirstSpawned;

            _spawned.Remove(traveledTrack);
            Destroy(traveledTrack);
        }

        private void ReleaseUnusedPickables()
        {
            foreach (PickableCube cube in _markedToDestroy)
                cube.Release();

            _markedToDestroy.Clear();
        }
    }
}