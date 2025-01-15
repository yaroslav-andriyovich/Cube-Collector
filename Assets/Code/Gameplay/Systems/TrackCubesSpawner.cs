using System;
using System.Collections.Generic;
using Code.Core.Services.Pools;
using Code.Core.Services.StaticData;
using Code.Gameplay.Cubes;
using Code.Gameplay.Tracks;
using UnityEngine;

namespace Code.Gameplay.Systems
{
    public class TrackCubesSpawner : IDisposable
    {
        private readonly TrackSpawner _trackSpawner;
        private readonly PoolService _poolService;
        private readonly ConfigService _configService;

        private readonly Dictionary<Cube, Track> _cubesAndTracks;

        public TrackCubesSpawner(TrackSpawner trackSpawner, PoolService poolService, ConfigService configService)
        {
            _trackSpawner = trackSpawner;
            _poolService = poolService;
            _configService = configService;
            _cubesAndTracks = new Dictionary<Cube, Track>();
        }

        public void Initialize()
        {
            _trackSpawner.Spawned += OnTrackSpawned;
            _trackSpawner.DeSpawned += OnTrackDeSpawned;
            
            _poolService.Warmup(_configService.GetTrackSpawner().cubePrefab, 20);
        }

        public void Dispose()
        {
            _trackSpawner.Spawned -= OnTrackSpawned;
            _trackSpawner.DeSpawned -= OnTrackDeSpawned;
            
            _cubesAndTracks.Clear();
        }

        private void OnTrackSpawned(Track track)
        {
            CubeSpawnPoint[] cubeSpawnPoints = track.GetCubeSpawnPoints();
            List<Cube> trackCubes = new List<Cube>(cubeSpawnPoints.Length);
            
            foreach (CubeSpawnPoint cubeSpawnPoint in cubeSpawnPoints)
            {
                Cube cube = SpawnCube(track.CubePrefab, cubeSpawnPoint.transform.position, cubeSpawnPoint.transform.rotation, track.transform);

                trackCubes.Add(cube);
                _cubesAndTracks.Add(cube, track);
            }
            
            track.AttachCubes(trackCubes);
        }

        private Cube SpawnCube(Cube prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            Cube cube = _poolService.Spawn(prefab, position, rotation, parent);
            cube.PickedUp += OnCubePickedUp;

            return cube;
        }

        private void OnCubePickedUp(Cube cube)
        {
            cube.PickedUp -= OnCubePickedUp;

            if (_cubesAndTracks.TryGetValue(cube, out Track track))
            {
                track.DetachCube(cube);
                _cubesAndTracks.Remove(cube);
            }
        }

        private void OnTrackDeSpawned(Track track)
        {
            IReadOnlyCollection<Cube> detachedCubes = track.DetachAllCubes();

            foreach (Cube cube in detachedCubes)
            {
                if (_cubesAndTracks.ContainsKey(cube))
                {
                    cube.PickedUp -= OnCubePickedUp;
                    _cubesAndTracks.Remove(cube);
                }
                
                cube.Release();
            }
        }
    }
}