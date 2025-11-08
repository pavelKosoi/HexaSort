using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : Poolable
{    
    public Color Color {  get; private set; }
    public HexView HexView {  get; private set; }

    public override void Init()
    {
        HexView = GetComponentInChildren<HexView>();
        base.Init();
    }

    public void SetColor(Color color)
    {
        Color = color;
        HexView.SetColor(color);
    }
    public override void ReturnToPool()
    {
        transform.localScale = Vector3.one;
        base.ReturnToPool();
    }
}
