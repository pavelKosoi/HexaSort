using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Action OnLevelComplete;
    public Action OnLevelFailed;

    public HexGrid HexGrid {  get; private set; }


    private void Awake()
    {
        HexGrid = GetComponentInChildren<HexGrid>();
    }
}
