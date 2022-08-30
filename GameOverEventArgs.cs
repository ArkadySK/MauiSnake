namespace MauiDemo
{
    public delegate void GameOverEventHandler(Object sender, GameOverEventArgs e);
    public class GameOverEventArgs: EventArgs
    {
        public int Score { get; set; }
        public TimeSpan PlayTime { get; set; } 
        
        public bool IsFinished { get; set; }
    }
}