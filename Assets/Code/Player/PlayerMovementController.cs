using System;
using Code.Core.Services.Input;
using UnityEngine;
using VContainer;

namespace Code.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private HorizontalBoundaries _horizontalBoundaries;
        [SerializeField, Min(0f)] private float _forwardSpeed;
        [SerializeField, Min(0f)] private float _horizontalSpeed;
        
        private IInputService _inputService;

        private void OnEnable() => 
            _rigidbody.velocity = Vector3.forward * _forwardSpeed;

        private void OnDisable() => 
            _rigidbody.isKinematic = true;

        private void FixedUpdate()
        {
            ChangeVelocity();
            LimitHorizontalPosition();
        }

        [Inject]
        private void Construct(IInputService playerInput) => 
            _inputService = playerInput;

        public void Enable() => 
            enabled = true;
        
        public void Disable() => 
            enabled = false;

        private void ChangeVelocity() => 
            _rigidbody.velocity = new Vector3(GetHorizontalVelocity(), 0f, _forwardSpeed);

        private float GetHorizontalVelocity()
        {
            float horizontalVelocity = GetVelocityHorizontalShift();
            
            horizontalVelocity = LimitVelocityHorizontalShift(horizontalVelocity);

            return horizontalVelocity;
        }

        private float GetVelocityHorizontalShift() => 
            _horizontalSpeed * _inputService.MovingDelta.x;

        private float LimitVelocityHorizontalShift(float horizontalShift)
        {
            if (IsOffTrackLeft())
                horizontalShift = Mathf.Max(0f, horizontalShift);

            if (IsOffTrackRight())
                horizontalShift = Mathf.Min(0f, horizontalShift);

            return horizontalShift;
        }

        private bool IsOffTrackLeft() => 
            _rigidbody.position.x <= _horizontalBoundaries.min;

        private bool IsOffTrackRight() => 
            _rigidbody.position.x >= _horizontalBoundaries.max;

        private void LimitHorizontalPosition()
        {
            Vector3 position = _rigidbody.position;

            position.x = Mathf.Clamp(position.x, _horizontalBoundaries.min, _horizontalBoundaries.max);

            _rigidbody.position = position;
        }

        [Serializable]
        public struct HorizontalBoundaries
        {
            public float min;
            public float max;
        }
    }
}