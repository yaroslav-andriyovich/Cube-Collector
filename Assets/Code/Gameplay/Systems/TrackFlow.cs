using System;
using System.Collections.Generic;
using System.Threading;
using Code.Core.Services.Pools;
using Code.Core.Services.StaticData;
using Code.Gameplay.Tracks;
using Code.StaticData;
using Cysharp.Threading.Tasks;

namespace Code.Gameplay.Systems
{
    public class TrackFlow : IDisposable
    {
        private readonly ConfigService _configService;
        private readonly TrackSpawner _spawner;
        private readonly TrackDeSpawner _deSpawner;
        private readonly TrackCubesSpawner _cubesSpawner;
        private readonly LinkedList<Track> _tracks;
        private readonly CancellationTokenSource _tokenSource;

        private TrackSpawningConfig _config;
        private bool _initialTracksSpawned;

        public TrackFlow(ConfigService configService, PoolService poolService)
        {
            _configService = configService;
            _spawner = new TrackSpawner(poolService, configService);
            _deSpawner = new TrackDeSpawner();
            _cubesSpawner = new TrackCubesSpawner(poolService, configService);
            _tracks = new LinkedList<Track>();
            _tokenSource = new CancellationTokenSource();
        }
        
        public void Initialize()
        {
            _config = _configService.GetForTracks();

            _spawner.Spawned += OnTrackSpawned;
            _deSpawner.DeSpawned += OnTrackDeSpawned;

            _spawner.Initialize();
            _spawner.SpawnInitialTracks(OnInitialTracksSpawned);
        }

        public void Dispose()
        {
            _spawner.Spawned -= OnTrackSpawned;
            _deSpawner.DeSpawned -= OnTrackDeSpawned;
        }

        private void OnTrackSpawned(Track track)
        {
            RegisterTrack(track);
            _cubesSpawner.OnTrackSpawned(track);
            
            if (_initialTracksSpawned)
                DeSpawnTraveled();
        }

        private async void DeSpawnTraveled()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_config.deSpawnDelay), cancellationToken: _tokenSource.Token);
            
            _deSpawner.DeSpawn(_tracks.Last.Value);
        }

        private void OnTrackDeSpawned(Track track)
        {
            UnRegisterTrack(track);
            _cubesSpawner.OnTrackDeSpawned(track);
        }

        private void RegisterTrack(Track track)
        {
            track.NextTrackTrigger += _spawner.SpawnNext;
            _tracks.AddFirst(track);
        }

        private void UnRegisterTrack(Track track)
        {
            track.NextTrackTrigger -= _spawner.SpawnNext;
            _tracks.Remove(track);
        }

        private void OnInitialTracksSpawned() => 
            _initialTracksSpawned = true;
    }
}