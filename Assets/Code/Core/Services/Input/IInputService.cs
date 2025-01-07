using System;
using UnityEngine;

namespace Code.Core.Services.Input
{
    public interface IInputService
    {
        event Action Touch;
        Vector2 MovingDelta { get; }

        void EnableInput();
        void DisableInput();
    }
}