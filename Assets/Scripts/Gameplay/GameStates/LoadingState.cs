using System.Collections;
using UnityEngine;

public class LoadingState : BaseGameState
{
    public LoadingState(GameManager gm) : base(gm) { }

    public override void Enter()
    {
        if (gameManager.CurrentLevel != null)
        {            
            GameObject.Destroy(gameManager.CurrentLevel.gameObject);
        }

        int targetLevel = gameManager.CurrentLevelIndex % ConfigsManager.Instance.LevelsConfig.Levels.Length;

        
        var levelConfig = ConfigsManager.Instance.LevelsConfig.Levels[targetLevel];

        var spawnedLvl = Object.Instantiate(levelConfig.LevelPrefab);
        var level = spawnedLvl.GetComponent<Level>();

        gameManager.OnLevelLoaded(level, levelConfig);
    }
}
