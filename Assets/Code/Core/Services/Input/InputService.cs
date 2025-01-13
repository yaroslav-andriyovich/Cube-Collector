using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Code.Core.Services.Input
{
    public class InputService : IInputService, IInitializable, IDisposable
    {
        public Vector2 MovingDelta { get; private set; }

        private GameInputActions.MainActions _mainActions;

        public void Initialize()
        {
            _mainActions = new GameInputActions().Main;
            
            _mainActions.TouchDelta.performed += OnTouchDelta;
        }

        public void Dispose() => 
            _mainActions.TouchDelta.performed -= OnTouchDelta;

        public void EnableInput() => 
            _mainActions.Enable();

        public void DisableInput() => 
            _mainActions.Disable();

        private void OnTouchDelta(InputAction.CallbackContext ctx) => 
            MovingDelta = _mainActions.TouchDelta.ReadValue<Vector2>();
    }
}