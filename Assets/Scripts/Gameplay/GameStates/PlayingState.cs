public class PlayingState : BaseGameState 
{ 
    public PlayingState(GameManager gm) : base(gm) { } 

    public override void Enter() 
    { 
        gameManager.CurrentLevel.OnLevelComplete += HandleLevelComplete; 
        gameManager.StacksBar.RefreshBar(); 
    } 
    public override void Exit() 
    { 
        gameManager.CurrentLevel.OnLevelComplete -= HandleLevelComplete; 
    } 
    private void HandleLevelComplete() 
    { 
        gameManager.OnLevelCompleted(); 
    } 
}