using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Code.Infrastructure.Services.InputService
{
    public class InputService : IInputService, IInitializable, IDisposable
    {
        public event Action Touch;
        public Vector2 MovingDelta { get; private set; }

        private GameInputActions.MainActions _mainActions;

        public void Initialize()
        {
            _mainActions = new GameInputActions().Main;
            
            _mainActions.FirstTouch.performed += OnFirstTouch;
            _mainActions.TouchDelta.performed += OnTouchDelta;
        }

        public void Dispose()
        {
            _mainActions.FirstTouch.performed -= OnFirstTouch;
            _mainActions.TouchDelta.performed -= OnTouchDelta;
        }

        public void EnableInput() => 
            _mainActions.Enable();

        public void DisableInput() => 
            _mainActions.Disable();

        private void OnFirstTouch(InputAction.CallbackContext ctx) => 
            Touch?.Invoke();

        private void OnTouchDelta(InputAction.CallbackContext ctx) => 
            MovingDelta = _mainActions.TouchDelta.ReadValue<Vector2>();
    }
}