using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Code.Tracks
{
    public class TrackGenerator : MonoBehaviour
    {
        [Header("Initial")]
        [SerializeField] private GameObject _playerTrackInstance;
        [SerializeField] private GameObject[] _trackVariants;
        [SerializeField, Min(0)] private int _initialCount;

        [Header("Spawning")]
        [SerializeField, Min(0f)] private float _downwardPositionShift;
        [SerializeField, Min(0f)] private float _forwardPositionShift;
        [SerializeField, Min(0f)] private float _animationDuration;
        [SerializeField] private Ease _animationEase;

        private GameObject FirstSpawned => _spawned[0];
        private GameObject LastSpawned => _spawned[^1];

        private List<GameObject> _spawned;
        private List<GameObject> _markedToDestroy;

        private void Awake()
        {
            Initialize();
            InstantiateInitialTracks();
        }

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
            for (int i = 0; i < _initialCount; i++)
            {
                Vector3 position = LastSpawned.transform.position;

                position = ShiftPositionForward(position);

                InstantiateRandomPrefab(position);
            }
        }

        private Vector3 ShiftPositionForward(Vector3 position)
        {
            position.z += _forwardPositionShift;
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
            int endIndex = _trackVariants.Length;
            int randomIndex = Random.Range(startIndex, endIndex);
            GameObject randomTrack = _trackVariants[randomIndex];

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
            position.y = -_downwardPositionShift;
            return position;
        }

        private void AnimateLift(GameObject track)
        {
            float targetPosition = FirstSpawned.transform.position.y;

            track.transform
                .DOMoveY(targetPosition, _animationDuration)
                .SetEase(_animationEase)
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