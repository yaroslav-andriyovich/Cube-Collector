using System.Collections.Generic;
using Code.StaticData;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace Code.Gameplay.Tracks
{
    public class TrackSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _playerTrackInstance;

        private GameObject FirstSpawned => _spawned[0];
        private GameObject LastSpawned => _spawned[^1];

        private TrackSpawningConfig _config;
        private List<GameObject> _spawned;
        private List<GameObject> _markedToDestroy;

        private void Awake()
        {
            Initialize();
            InstantiateInitialTracks();
        }

        [Inject]
        private void Construct(TrackSpawningConfig config) => 
            _config = config;

        public void GenerateNext()
        {
            Vector3 position = GetNextPosition();
            GameObject spawnedTrack = InstantiateRandomPrefab(position);

            AnimateLift(spawnedTrack);
        }

        public void MarkToGarbageCollector(GameObject objectToDestroy) =>
            _markedToDestroy.Add(objectToDestroy);

        private void Initialize()
        {
            _spawned = new List<GameObject>
            {
                _playerTrackInstance
            };

            _markedToDestroy = new List<GameObject>();
        }

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
            GameObject track = Instantiate(prefab, at, Quaternion.identity);

            _spawned.Add(track);
            return track;
        }

        private GameObject GetRandomPrefab()
        {
            int startIndex = 0;
            int endIndex = _config.trackVariants.Length;
            int randomIndex = Random.Range(startIndex, endIndex);
            GameObject randomTrack = _config.trackVariants[randomIndex];

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
            DestroyOtherGarbage();
        }

        private void DestroyTraveledTrack()
        {
            GameObject traveledTrack = FirstSpawned;

            _spawned.Remove(traveledTrack);
            Destroy(traveledTrack);
        }

        private void DestroyOtherGarbage()
        {
            foreach (GameObject garbage in _markedToDestroy)
                Destroy(garbage);

            _markedToDestroy.Clear();
        }
    }
}