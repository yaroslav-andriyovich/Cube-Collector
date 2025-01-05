using System;
using UnityEngine;

namespace Code.Infrastructure.Services.Input
{
    public interface IInputService
    {
        event Action Touch;
        Vector2 MovingDelta { get; }

        void EnableInput();
        void DisableInput();
    }
}