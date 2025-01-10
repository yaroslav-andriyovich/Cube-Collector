namespace Code.Gameplay.Services.GameControl
{
    public interface IGameControl : IGameEventSender
    {
        public void StartGame();
        public void EndGame();
        public void RestartGame();
    }
}