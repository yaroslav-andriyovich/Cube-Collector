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
        [SerializeField] private CubeSpawnPoint[] _pickableSpawnPoints;
        [SerializeField] private Transform _pickableParent;

        private List<PickableCube> _spawnedPickables = new List<PickableCube>();

        private void OnDestroy()
        {
            foreach (PickableCube cube in _spawnedPickables)
            {
                cube.PickedUp -= OnCubePickedUp;
                cube.Release();
            }
            
            _spawnedPickables.Clear();
        }

        [Inject]
        private void Construct(PoolService poolService)
        {
            MonoPool<PickableCube> pickablesPool = poolService.GetRandomPool<PickableCube>();

            foreach (CubeSpawnPoint point in GetPickableSpawnPoints())
            {
                PickableCube cube = pickablesPool.Get();

                cube.transform.position = point.transform.position;
                cube.transform.rotation = point.transform.rotation;
                cube.transform.SetParent(_pickableParent);
                cube.PickedUp += OnCubePickedUp;
                
                _spawnedPickables.Add(cube);
            }
        }

        private void OnCubePickedUp(PickableCube cube)
        {
            cube.PickedUp -= OnCubePickedUp;
            
            _spawnedPickables.Remove(cube);
        }

        public CubeSpawnPoint[] GetPickableSpawnPoints() => 
            _pickableSpawnPoints;
    }
}