using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    #region Fields
    [SerializeField] private StacksBar stacksBar;
    #endregion
    #region Properties
    public StateMachine StateMachine { get; private set; }
    public Level CurrentLevel { get; private set; }
    #endregion

    #region Getters
    public StacksBar StacksBar => stacksBar;
    #endregion

    #region UnityMethodes
    private void Start()
    {
        StateMachine = new StateMachine();
     
        StateMachine.RegisterState(new LoadingState(this));
        StateMachine.RegisterState(new PlayingState(this));
          
        StateMachine.ChangeState<LoadingState>();
    }

    private void Update()
    {
        StateMachine.Update();
    }
    #endregion

    #region GameManagement
    public void OnLevelLoaded(Level level)
    {
        CurrentLevel = level;
        StateMachine.ChangeState<PlayingState>();
    }

    public void OnLevelCompleted()
    {

    }

    public void LoadNextLevel()
    {
        StateMachine.ChangeState<LoadingState>();
    }
    #endregion
}
