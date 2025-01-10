using System;
using System.Collections.Generic;
using Code.Core.Pools;
using Code.Core.Services.Pools;
using Code.Core.Services.Vibration;
using Code.Gameplay.Cubes;
using Code.Gameplay.Environment;
using Code.Gameplay.Tracks;
using Code.VFX;
using UnityEngine;
using VContainer;

namespace Code.Player
{
    public class CubeHolder : MonoBehaviour
    {
        [SerializeField] private List<PickableCube> _stack;
        [SerializeField, Min(0)] private int _stackCapacity;
        [SerializeField] private ParticleSystem _stackParticle;
        [SerializeField] private CubeCollectionText _collectTextPrefab;
        [SerializeField, Min(0)] private int _vibrationMilliseconds;

        public event Action<Vector3> NewPlayerPosition;
        public event Action Emptied;

        private CameraShaker _cameraShaker;
        private TrackSpawner _trackSpawner;
        private IVibrationService _vibrationService;
        private MonoPool<CubeCollectionText> _collectTextPool;

        private void Start()
        {
            _trackSpawner = FindObjectOfType<TrackSpawner>();
            SubscribeInitialCubes();
        }

        private void OnDestroy()
        {
            UnSubscribeAll();
        }

        [Inject]
        private void Construct(IVibrationService vibrationService, CameraShaker cameraShaker, PoolService poolService)
        {
            _vibrationService = vibrationService;
            _cameraShaker = cameraShaker;

            _collectTextPool = poolService.CreatePool(_collectTextPrefab);
            _collectTextPool.Warmup(3);
        }

        private void SubscribeInitialCubes()
        {
            foreach (PickableCube cube in _stack)
            {
                SubscribeCube(cube);
                cube.PickUp();
            }
        }

        private void UnSubscribeAll()
        {
            foreach (PickableCube cube in _stack)
                UnSubscribeCube(cube);
        }

        private void SubscribeCube(PickableCube cube)
        {
            cube.CollisionWithDropped += OnCollisionWithDropped;
            cube.WallCollision += OnWallCollision;
        }
        
        private void UnSubscribeCube(PickableCube cube)
        {
            cube.CollisionWithDropped -= OnCollisionWithDropped;
            cube.WallCollision -= OnWallCollision;
        }

        private void OnCollisionWithDropped(PickableCube other)
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

        private void DestroyDroppedCube(PickableCube cube)
        {
            _stackParticle.Stop();
            _stackParticle.transform.position = cube.transform.position;
            _stackParticle.Play();

            cube.PickUp();
            cube.Release();
        }

        private void AddCube(PickableCube cube)
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

        private bool CollisionIsRegistered(PickableCube cube) => 
            _stack.Contains(cube);

        private void MakeCubeAsChildren(PickableCube cube) => 
            cube.transform.SetParent(transform);

        private void RaiseCube(PickableCube cube, out Vector3 newCubePosition)
        {
            newCubePosition = GetNewCubePosition(cube);
            cube.transform.localPosition = newCubePosition;
            
            NewPlayerPosition?.Invoke(newCubePosition + Vector3.up * cube.Height);
        }

        private Vector3 GetNewCubePosition(PickableCube cube)
        {
            PickableCube preLastCubeInStack = _stack[^2];
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

            _collectTextPool.Get(worldPosition, Quaternion.identity);
        }

        private void OnWallCollision(PickableCube owner)
        {
            RemoveCube(owner);
            _cameraShaker.LightShake();
            _vibrationService.Vibrate(_vibrationMilliseconds);
        }

        private void RemoveCube(PickableCube cube)
        {
            if (cube == null || !CollisionIsRegistered(cube))
                return;

            UnSubscribeCube(cube);
            _stack.Remove(cube);
            cube.transform.SetParent(null);
            _trackSpawner.MarkUnusedPickable(cube);
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