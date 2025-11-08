using System.Collections;
using UnityEngine;

public class LoadingState : BaseGameState
{
    public LoadingState(GameManager gm) : base(gm) { }

    public override void Enter()
    {
        var spawnedLvl = Object.Instantiate(Resources.Load("Levels/Level 0") as GameObject);
        var level = spawnedLvl.GetComponent<Level>();
        gameManager.OnLevelLoaded(level);
    }
}
