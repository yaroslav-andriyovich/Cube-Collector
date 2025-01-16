using System;
using Code.Core.Services.Pools;
using Code.Core.Services.StaticData;
using Code.Gameplay.Tracks;
using Code.StaticData;
using DG.Tweening;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Code.Gameplay.Systems
{
    public class TrackSpawner : IInitializable
    {
        public event Action<Track> Spawned;

        private readonly PoolService _poolService;
        private readonly ConfigService _configService;

        private TrackSpawningConfig _config;
        private Track _lastSpawned;

        public TrackSpawner(PoolService poolService, ConfigService configService)
        {
            _poolService = poolService;
            _configService = configService;
        }

        public void Initialize() => 
            _config = _configService.GetForTracks();

        public void SpawnInitialTracks(Action callback)
        {
            if (_lastSpawned is not null)
                throw new InvalidOperationException("[Track Spawner] The initial tracks had already been created");
            
            SpawnPlayerStartTrack();

            for (int i = 0; i < _config.initialCount; i++)
                SpawnNextRandomTrack();
            
            callback?.Invoke();
        }

        public void SpawnNext()
        {
            if (_lastSpawned is null)
                throw new NullReferenceException("[Track Spawner] Not found last track");
            
            Track spawnedTrack = SpawnNextRandomTrack();

            AnimateLift(spawnedTrack);
        }

        private void SpawnPlayerStartTrack() => 
            SpawnTrack(_config.initialTrackPrefab, _config.initialTrackPosition, _config.initialTrackRotation);

        private Track SpawnNextRandomTrack() => 
            SpawnTrack(GetRandomPrefab(), GetNextPosition(), _lastSpawned.transform.rotation);

        private Track SpawnTrack(Track prefab, Vector3 at, Quaternion rotation)
        {
            Track track = _poolService.Spawn(prefab, at, rotation);

            _lastSpawned = track;
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

        private Vector3 GetNextPosition() => 
            ShiftPositionForward(_lastSpawned.transform.position);

        private Vector3 ShiftPositionForward(Vector3 position)
        {
            position.z += _config.forwardPositionShift;
            return position;
        }

        private void AnimateLift(Track track)
        {
            Vector3 targetPosition = track.transform.position;

            track.transform.position = ShiftPositionDown(targetPosition);
            
            track.transform
                .DOMoveY(targetPosition.y, _config.animationDuration)
                .SetEase(_config.animationEase)
                .SetLink(track.gameObject);
        }

        private Vector3 ShiftPositionDown(Vector3 position)
        {
            position.y = -_config.downwardPositionShift;
            return position;
        }
    }
}