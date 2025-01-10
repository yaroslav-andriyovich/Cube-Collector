using System;

namespace Code.Gameplay.Services.GameControl
{
    public interface IGameEventSender
    {
        event Action GameStarted;
        event Action GameEnded;
    }
}