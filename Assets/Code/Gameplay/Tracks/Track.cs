using System;
using System.Collections.Generic;
using System.Linq;
using Code.Core.Pools.Poolable;
using Code.Gameplay.Cubes;
using UnityEngine;

namespace Code.Gameplay.Tracks
{
    public class Track : PoolableBase
    {
        [field: SerializeField] public Cube CubePrefab { get; private set; }
        [SerializeField] private CubeSpawnPoint[] _cubeSpawnPoints;
        [SerializeField] private TrackSpawnTrigger _trackSpawnTrigger;

        public event Action NextTrackTrigger
        {
            add => _trackSpawnTrigger.Triggered += value;
            remove => _trackSpawnTrigger.Triggered -= value;
        }

        private readonly List<Cube> _attachedCubes = new List<Cube>();

        private void Awake() => 
            _cubeSpawnPoints ??= Array.Empty<CubeSpawnPoint>();

        #if UNITY_EDITOR
        public void SetCubeSpawnPoints(CubeSpawnPoint[] spawnPoints) => 
            _cubeSpawnPoints = spawnPoints;
        #endif

        public CubeSpawnPoint[] GetCubeSpawnPoints() => 
            _cubeSpawnPoints;

        public void AttachCube(Cube cube)
        {
            if (_attachedCubes.Contains(cube))
                return;
            
            if (cube.transform.parent != transform)
                cube.transform.SetParent(transform);

            _attachedCubes.Add(cube);
        }
        
        public void AttachCubes(List<Cube> cubes)
        {
            foreach (Cube cube in cubes)
                AttachCube(cube);
        }
        
        public void DetachCube(Cube cube)
        {
            if (!_attachedCubes.Contains(cube))
                return;

            _attachedCubes.Remove(cube);
        }

        public IReadOnlyCollection<Cube> DetachAllCubes()
        {
            IReadOnlyList<Cube> cubes = _attachedCubes.ToList();
            
            _attachedCubes.Clear();

            return cubes;
        }
    }
}