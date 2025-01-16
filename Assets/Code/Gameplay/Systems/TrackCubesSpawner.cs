using System.Collections.Generic;
using Code.Core.Services.Pools;
using Code.Core.Services.StaticData;
using Code.Gameplay.Cubes;
using Code.Gameplay.Tracks;
using Code.StaticData;
using UnityEngine;
using VContainer.Unity;

namespace Code.Gameplay.Systems
{
    public class TrackCubesSpawner : IInitializable
    {
        private readonly PoolService _poolService;
        private readonly ConfigService _configService;

        private readonly Dictionary<Cube, Track> _cubesAndTracks;

        public TrackCubesSpawner(PoolService poolService, ConfigService configService)
        {
            _poolService = poolService;
            _configService = configService;
            _cubesAndTracks = new Dictionary<Cube, Track>();
        }

        public void Initialize()
        {
            TrackSpawningConfig config = _configService.GetForTracks();
            
            _poolService.Warmup(config.cubePrefab, config.preparedCubesNumber);
        }

        public void OnTrackSpawned(Track track)
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

        public void OnTrackDeSpawned(Track track)
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
    }
}