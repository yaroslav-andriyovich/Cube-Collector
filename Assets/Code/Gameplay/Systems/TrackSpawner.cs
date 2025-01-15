using System;
using System.Collections;
using System.Collections.Generic;
using Code.Core.Services.Pools;
using Code.Core.Services.StaticData;
using Code.Gameplay.Tracks;
using Code.StaticData;
using DG.Tweening;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Code.Gameplay.Systems
{
    public class TrackSpawner : MonoBehaviour
    {
        [SerializeField] private Track _playerTrackInstance;

        public event Action<Track> Spawned;
        public event Action<Track> DeSpawned;

        private Track FirstSpawned => _spawned[0];
        private Track LastSpawned => _spawned[^1];

        private ConfigService _configService;
        private TrackSpawningConfig _config;
        private List<Track> _spawned;

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
            
            _spawned = new List<Track>
            {
                _playerTrackInstance
            };

            SubscribeInitialTracks();
            InstantiateInitialTracks();
        }

        public void GenerateNext()
        {
            Vector3 position = GetNextPosition();
            Track spawnedTrack = InstantiateRandomPrefab(position);

            AnimateLift(spawnedTrack);
        }

        private void SubscribeInitialTracks()
        {
            foreach (Track track in _spawned)
                track.NextTrackTrigger += GenerateNext;
        }
        
        private void UnSubscribeTrack(Track track) => 
            track.NextTrackTrigger -= GenerateNext;

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

        private Track InstantiateRandomPrefab(Vector3 at)
        {
            Track prefab = GetRandomPrefab();
            Track track = _objectResolver.Instantiate(prefab, at, Quaternion.identity);

            _spawned.Add(track);
            track.NextTrackTrigger += GenerateNext;
            Spawned?.Invoke(track);
            return track;
        }

        private Track GetRandomPrefab()
        {
            int startIndex = 0;
            int endIndex = _config.trackVariants.Length;
            int randomIndex = Random.Range(startIndex, endIndex);
            Track randomTrack = _config.trackVariants[randomIndex];

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

        private void AnimateLift(Track track)
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
        }

        private void DestroyTraveledTrack()
        {
            Track traveledTrack = FirstSpawned;

            UnSubscribeTrack(traveledTrack);

            _spawned.Remove(traveledTrack);
            DeSpawned?.Invoke(traveledTrack);
            Destroy(traveledTrack.gameObject);
        }
    }
}