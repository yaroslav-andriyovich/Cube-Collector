using System;
using System.Collections.Generic;
using Code.Core.Services.Pools;
using Code.Gameplay.Cubes;
using Code.Gameplay.Tracks;
using Code.VFX;
using UnityEngine;
using VContainer;

namespace Code.PlayerLogic
{
    public class CubeHolder : MonoBehaviour
    {
        [SerializeField] private List<Cube> _stack;
        [SerializeField, Min(0)] private int _stackCapacity;
        [SerializeField] private ParticleSystem _stackParticle;
        [SerializeField] private CubeCollectionText _collectTextPrefab;

        public event Action<Vector3> NewPlayerPosition;
        public event Action Emptied;
        public event Action CubeCollidedWithWall;

        private TrackSpawner _trackSpawner;
        private PoolService _poolService;

        private void Start() => 
            SubscribeInitialCubes();

        private void OnDestroy() => 
            UnSubscribeAll();

        [Inject]
        private void Construct(PoolService poolService, TrackSpawner trackSpawner)
        {
            _poolService = poolService;
            _poolService.Warmup(_collectTextPrefab, 3);
            
            _trackSpawner = trackSpawner;
        }

        public void ReleaseAll() => 
            UnSubscribeAll();

        private void SubscribeInitialCubes()
        {
            foreach (Cube cube in _stack)
            {
                SubscribeCube(cube);
                cube.PickUp();
            }
        }

        private void UnSubscribeAll()
        {
            foreach (Cube cube in _stack)
                UnSubscribeCube(cube);
        }

        private void SubscribeCube(Cube cube)
        {
            cube.CollisionWithDropped += OnCollisionWithDropped;
            cube.WallCollision += OnWallCollision;
        }
        
        private void UnSubscribeCube(Cube cube)
        {
            cube.CollisionWithDropped -= OnCollisionWithDropped;
            cube.WallCollision -= OnWallCollision;
        }

        private void OnCollisionWithDropped(Cube other)
        {
            if (CollisionIsRegistered(other))
                return;

            if (IsStackOverflow())
            {
                DestroyDroppedCube(other);
                return;
            }

            AddCube(other);
        }

        private bool IsStackOverflow() => 
            _stack.Count >= _stackCapacity;

        private void DestroyDroppedCube(Cube cube)
        {
            _stackParticle.Stop();
            _stackParticle.transform.position = cube.transform.position;
            _stackParticle.Play();

            cube.PickUp();
            cube.Release();
        }

        private void AddCube(Cube cube)
        {
            if (CollisionIsRegistered(cube))
                return;

            cube.PickUp();
            _stack.Add(cube);
            SubscribeCube(cube);
            MakeCubeAsChildren(cube);
            RaiseCube(cube, out Vector3 cubePosition);
            PlayStackEffect(cubePosition);
            SpawnCollectText(cubePosition);
        }

        private bool CollisionIsRegistered(Cube cube) => 
            _stack.Contains(cube);

        private void MakeCubeAsChildren(Cube cube) => 
            cube.transform.SetParent(transform);

        private void RaiseCube(Cube cube, out Vector3 newCubePosition)
        {
            newCubePosition = GetNewCubePosition(cube);
            cube.transform.localPosition = newCubePosition;
            
            NewPlayerPosition?.Invoke(newCubePosition + Vector3.up * cube.Height);
        }

        private Vector3 GetNewCubePosition(Cube cube)
        {
            Cube preLastCubeInStack = _stack[^2];
            Vector3 prevCubePosition = preLastCubeInStack.transform.localPosition;
            Vector3 newCubePosition = prevCubePosition + Vector3.up * cube.Height;
            
            return newCubePosition;
        }

        private void PlayStackEffect(Vector3 at)
        {
            _stackParticle.Stop();
            _stackParticle.transform.localPosition = at;
            _stackParticle.Play();
        }

        private void SpawnCollectText(Vector3 at)
        {
            Vector3 worldPosition = transform.TransformPoint(at);
            
            _poolService.Spawn(_collectTextPrefab, worldPosition, Quaternion.identity);
        }

        private void OnWallCollision(Cube owner)
        {
            RemoveCube(owner);
            CubeCollidedWithWall?.Invoke();
        }

        private void RemoveCube(Cube cube)
        {
            if (cube == null || !CollisionIsRegistered(cube))
                return;

            UnSubscribeCube(cube);
            _stack.Remove(cube);
            cube.transform.SetParent(null);
            _trackSpawner.MarkUnusedCube(cube);
            CheckStackEmptying();
        }

        private void CheckStackEmptying()
        {
            if (_stack.Count > 0)
                return;
            
            Emptied?.Invoke();
        }
    }
}