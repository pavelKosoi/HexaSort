using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StacksBar : MonoBehaviour
{
    #region Fields
    [SerializeField] Transform[] stackPoints;
    [SerializeField] GameObject stackPrefab;

    List<Stack> stacks = new List<Stack>();
    
    #endregion

    #region Getters
    public List<Stack> Stacks => stacks;
    #endregion

    public void RefreshBar()
    {
        StartCoroutine(SpawnStacks());
    }

    public void Clear()
    {
        foreach (var item in stacks)
        {
            Destroy(item.gameObject);
        }
        stacks.Clear();
    }

    IEnumerator SpawnStacks()
    {
        for (int i = 0; i < 3; i++)
        {
            var spawnPoint = stackPoints.OrderBy(p => p.position.x).FirstOrDefault().position + Vector3.right * 10f;
            var stack = Instantiate(stackPrefab, spawnPoint, Quaternion.identity).GetComponent<Stack>();
            stack.Build();
            stacks.Add(stack);
            stack.SetIdlePosition(stackPoints[i].position);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void RemoveStack(Stack stack)
    {
        stacks.Remove(stack);
        if(stacks.Count == 0) RefreshBar();
    }
}
