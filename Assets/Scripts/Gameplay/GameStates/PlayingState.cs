public class PlayingState : BaseGameState 
{ 
    public PlayingState(GameManager gm) : base(gm) { } 

    public override void Enter() 
    {
        gameManager.CurrentLevel.OnLevelComplete += HandleLevelComplete;         
        gameManager.CurrentLevel.OnLevelFailed += HandleLevelFailed;

        gameManager.MainUIManager.OpenScreen(MainUIManager.ScreenType.GameplayScreen);
        gameManager.StacksBar.RefreshBar(); 
    } 
    public override void Exit() 
    { 
        gameManager.CurrentLevel.OnLevelComplete -= HandleLevelComplete; 
        gameManager.CurrentLevel.OnLevelFailed -= HandleLevelFailed;
    } 
    void HandleLevelComplete() 
    {
        gameManager.MainUIManager.OpenScreen(MainUIManager.ScreenType.LevelCompleteScreen);
        gameManager.OnLevelCompleted(); 
    }
    void HandleLevelFailed()
    {
        gameManager.MainUIManager.OpenScreen(MainUIManager.ScreenType.LevelFailedScreen);
    }
}