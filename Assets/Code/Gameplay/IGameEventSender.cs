using System;

namespace Code.Gameplay
{
    public interface IGameEventSender
    {
        event Action GameStarted;
        event Action GameEnded;
    }
}