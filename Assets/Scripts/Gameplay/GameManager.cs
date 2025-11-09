using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    #region Fields
    [SerializeField] StacksBar stacksBar;
    [SerializeField] HexStackAnimator animator;
    [SerializeField] MainUIManager mainUIManager;
    [SerializeField] BoostersManager boosterManager;
    [SerializeField] CameraController cameraController;
    [SerializeField] InputManager inputManager;
    [SerializeField] SoundsManager soundsManager;

    public Action OnlevelLoadingSarted;
    public Action OnlevelLoadingCompleted;

    #endregion

    #region Properties
    public StateMachine StateMachine { get; private set; }
    public Level CurrentLevel { get; private set; }
    public LevelConfig CurrentLevelConfig { get; private set; }
    public int CurrentLevelIndex
    {
        get => PlayerPrefs.GetInt("Level");
        private set => PlayerPrefs.SetInt("Level", value);
    }

    #endregion

    #region Getters
    public StacksBar StacksBar => stacksBar;
    public HexStackAnimator HexStackAnimator => animator;
    public MainUIManager MainUIManager => mainUIManager;
    public BoostersManager BoosterManager => boosterManager;
    public CameraController CameraController => cameraController;
    public InputManager InputManager => inputManager;
    public SoundsManager SoundsManager => soundsManager;
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
    public void OnLevelLoaded(Level level, LevelConfig levelConfig)
    {
        CurrentLevel = level;
        CurrentLevelConfig = levelConfig;
        OnlevelLoadingCompleted?.Invoke();
        StateMachine.ChangeState<PlayingState>();
    }

    public void OnLevelCompleted()
    {
        CurrentLevelIndex++;        
    }

    public void MoveToNextLevel()
    {
        stacksBar.Clear();
        LoadNextLevel();
    }  

    void LoadNextLevel()
    {
        StateMachine.ChangeState<LoadingState>();
        OnlevelLoadingSarted?.Invoke();
    }
    #endregion
}
