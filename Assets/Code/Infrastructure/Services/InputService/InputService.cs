using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Code.Infrastructure.Services.InputService
{
    public class InputService : IInitializable, IDisposable
    {
        public GameInputActions.MainActions Actions => _actions;
        public Vector2 MovingDelta { get; private set; }

        private GameInputActions.MainActions _actions;

        public void Initialize()
        {
            _actions = new GameInputActions().Main;
            
            _actions.TouchDelta.performed += OnTouchDelta;
            
            EnableInput();
        }

        public void EnableInput() => 
            _actions.Enable();

        public void DisableInput() => 
            _actions.Disable();

        public void Dispose() => 
            _actions.TouchDelta.performed -= OnTouchDelta;

        private void OnTouchDelta(InputAction.CallbackContext ctx) => 
            MovingDelta = _actions.TouchDelta.ReadValue<Vector2>();
    }
}