using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexView : Poolable
{
    Material material;

    public override void Init()
    {
        var renderer = GetComponentInChildren<MeshRenderer>();
        material = new Material(renderer.material);
        renderer.material = material;

        base.Init();
    }

    public void SetColor(Color color) => StartCoroutine(SetColorRoutine(color));
    IEnumerator SetColorRoutine(Color color)
    {
        yield return new WaitWhile(() => !Inited);
        material.color = color;
    }

}
