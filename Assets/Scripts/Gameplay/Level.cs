using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    #region Fields

    MatchSelector matchSelector;
    Coroutine matchRoutine;
    int points;    

    public Action OnLevelComplete;
    public Action OnLevelFailed;
    public static Action<float> OnLevelProgressChanged;
    #endregion

    #region Properties
    public HexGrid HexGrid {  get; private set; }

    #endregion

    #region UnityMethodes
    private void Awake()
    {
        HexGrid = GetComponentInChildren<HexGrid>();
        matchSelector = new MatchSelector(HexGrid);

    }
    #endregion

    #region PropgressManagement

    public void AddPoints(int amount)
    {
        points += amount;
        OnLevelProgressChanged?.Invoke(points);
        if (points >= GameManager.Instance.CurrentLevelConfig.TargetPoints)
        {
            OnLevelComplete?.Invoke();
        }
    }

    #endregion

    #region MatchesChecking

    public void CheckAllMatches()
    {
        if (matchRoutine != null) return;
        matchRoutine = StartCoroutine(CheckAllMatchesCoroutine());
    }

    private IEnumerator CheckAllMatchesCoroutine()
    {
        bool hasMatches;

        do
        {
            hasMatches = false;

            foreach (var cell in HexGrid.AllCells)
            {
                if (!cell.IsOccupied) continue;

                var match = matchSelector.FindBestMatchPair(cell);
                if (match == null) continue;

                hasMatches = true;

                var (from, to) = match.Value;

                Hex[] stackHexes = from.Hexes.Reverse<Hex>()
                    .TakeWhile(h => h.Color == from.UpperColor).Reverse().ToArray();

                bool done = false;

                GameManager.Instance.HexStackAnimator.RunHexesMoving(new HexStackAnimator.AnimationInfo
                {
                    from = from,
                    to = to,
                    hexes = stackHexes,
                    onComplete = () =>
                    {
                        from.TryToDestroy();
                        to.TryToPop();
                        done = true;
                    }
                });

                yield return new WaitUntil(() => done);
            }

        } while (hasMatches);

        if (HexGrid.AllCellsFilled)
        {            
            OnLevelFailed?.Invoke();
        }

        matchRoutine = null;
    }
    #endregion

}
