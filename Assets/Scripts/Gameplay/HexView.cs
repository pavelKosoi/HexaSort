using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class HexView : MonoBehaviour
{
    Material material;

    public bool Inited {  get; private set; }

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        var renderer = GetComponent<MeshRenderer>();
        material = new Material(renderer.material);
        renderer.material = material;
        Inited = true;
    }

    public void SetColor(Color color) => StartCoroutine(SetColorRoutine(color));
    IEnumerator SetColorRoutine(Color color)
    {
        yield return new WaitWhile(() => !Inited);
        material.color = color;
    }
}
