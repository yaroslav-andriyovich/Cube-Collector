namespace Code.Gameplay
{
    public interface IGameControl : IGameEventSender
    {
        public void StartGame();
        public void EndGame();
        public void RestartGame();
    }
}