using System.Collections.Generic;
using Code.Core.Pools;
using Code.Core.Services.Pools;
using Code.Gameplay.Cubes;
using UnityEngine;
using VContainer;

namespace Code.Gameplay.Tracks
{
    public class Track : MonoBehaviour
    {
        [SerializeField] private CubeSpawnPoint[] _cubeSpawnPoints;

        private List<Cube> _spawnedCubes = new List<Cube>();

        private void OnDestroy()
        {
            foreach (Cube cube in _spawnedCubes)
            {
                cube.PickedUp -= OnCubePickedUp;
                cube.Release();
            }
            
            _spawnedCubes.Clear();
        }

        [Inject]
        private void Construct(PoolService poolService)
        {
            MonoPool<Cube> cubesPool = poolService.GetRandomPool<Cube>();

            foreach (CubeSpawnPoint point in GetCubeSpawnPoints())
            {
                Cube cube = cubesPool.Get();

                cube.transform.position = point.transform.position;
                cube.transform.rotation = point.transform.rotation;
                cube.transform.SetParent(transform);
                cube.PickedUp += OnCubePickedUp;
                
                _spawnedCubes.Add(cube);
            }
        }

        #if UNITY_EDITOR
        public void SetCubeSpawnPoints(CubeSpawnPoint[] spawnPoints) => 
            _cubeSpawnPoints = spawnPoints;
        #endif

        public CubeSpawnPoint[] GetCubeSpawnPoints() => 
            _cubeSpawnPoints;

        private void OnCubePickedUp(Cube cube)
        {
            cube.PickedUp -= OnCubePickedUp;
            
            _spawnedCubes.Remove(cube);
        }
    }
}