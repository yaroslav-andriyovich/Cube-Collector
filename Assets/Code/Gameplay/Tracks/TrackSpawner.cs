using System.Collections;
using System.Collections.Generic;
using Code.Core.Services.Pools;
using Code.Core.Services.StaticData;
using Code.Gameplay.Cubes;
using Code.StaticData;
using DG.Tweening;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Code.Gameplay.Tracks
{
    public class TrackSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _playerTrackInstance;

        private GameObject FirstSpawned => _spawned[0];
        private GameObject LastSpawned => _spawned[^1];

        private ConfigService _configService;
        private TrackSpawningConfig _config;
        private List<GameObject> _spawned;
        private List<PickableCube> _markedToDestroy;

        private PoolService _poolService;
        private IObjectResolver _objectResolver;

        [Inject]
        private void Construct(ConfigService configService, PoolService poolService, IObjectResolver objectResolver)
        {
            _configService = configService;
            _poolService = poolService;
            _objectResolver = objectResolver;
        }

        public void Initialize()
        {
            _config = _configService.GetTrackSpawner();
            
            _spawned = new List<GameObject>
            {
                _playerTrackInstance
            };

            _markedToDestroy = new List<PickableCube>();
            
            _poolService.Warmup(_config.pickableCubePrefab, 20);
            
            InstantiateInitialTracks();
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
                .SetLink(track.gameObject)
                .OnComplete(CollectGarbage);
        }

        private void CollectGarbage() => 
            StartCoroutine(GarbageCollectionRoutine());

        private IEnumerator GarbageCollectionRoutine()
        {
            yield return new WaitForSeconds(1f);
            
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