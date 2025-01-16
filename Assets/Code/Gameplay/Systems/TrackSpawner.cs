using System;
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
    public class TrackSpawner : IInitializable
    {
        public event Action<Track> Spawned;
        public event Action<Track> DeSpawned;

        private Track FirstSpawned => _spawned[0];
        private Track LastSpawned => _spawned[^1];

        private ConfigService _configService;
        private TrackSpawningConfig _config;
        private List<Track> _spawned;

        private PoolService _poolService;

        [Inject]
        private void Construct(ConfigService configService, PoolService poolService)
        {
            _configService = configService;
            _poolService = poolService;
            _spawned = new List<Track>();
        }

        public void Initialize() => 
            _config = _configService.GetForTracks();

        public void SpawnInitialTracks()
        {
            if (_spawned.Count > 0)
                throw new InvalidOperationException("[Track Spawner] The initial tracks had already been created");
            
            SpawnPlayerStartTrack();

            for (int i = 0; i < _config.initialCount; i++)
                SpawnTrack(GetRandomPrefab(), GetNextPosition(), LastSpawned.transform.rotation);
        }

        public void SpawnNext()
        {
            Track spawnedTrack = SpawnTrack(GetRandomPrefab(), GetNextPosition(), LastSpawned.transform.rotation);

            AnimateLift(spawnedTrack);
        }

        private void SpawnPlayerStartTrack() => 
            SpawnTrack(_config.initialTrackPrefab, _config.initialTrackPosition, _config.initialTrackRotation);

        private Track SpawnTrack(Track prefab, Vector3 at, Quaternion rotation)
        {
            Track track = _poolService.Spawn(prefab, at, rotation);

            RegisterTrack(track);

            return track;
        }

        private void RegisterTrack(Track track)
        {
            track.NextTrackTrigger += SpawnNext;
            _spawned.Add(track);
            Spawned?.Invoke(track);
        }

        private void UnRegisterTrack(Track track) => 
            track.NextTrackTrigger -= SpawnNext;

        private Vector3 GetNextPosition() => 
            ShiftPositionForward(LastSpawned.transform.position);

        private Track GetRandomPrefab()
        {
            int startIndex = 0;
            int endIndex = _config.trackVariants.Length;
            int randomIndex = Random.Range(startIndex, endIndex);
            Track randomTrack = _config.trackVariants[randomIndex];

            return randomTrack;
        }

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
                .SetLink(track.gameObject)
                .OnComplete(CollectGarbage);
        }

        private Vector3 ShiftPositionDown(Vector3 position)
        {
            position.y = -_config.downwardPositionShift;
            return position;
        }

        private void CollectGarbage() => 
            DOVirtual.DelayedCall(1f, DestroyTraveledTrack);

        private void DestroyTraveledTrack()
        {
            Track traveledTrack = FirstSpawned;

            UnRegisterTrack(traveledTrack);
            _spawned.Remove(traveledTrack);
            DeSpawned?.Invoke(traveledTrack);
            traveledTrack.Release();
        }
    }
}