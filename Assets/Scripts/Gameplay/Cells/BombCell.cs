using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCell : BaseHexCell
{
    Poolable bomb;
    bool exploded;

    Vector3 targetPos
    {
        get
        {
            var targetpoint = IsOccupied ? upperPoint : transform.position;
            Vector3 screenPos = GameManager.Instance.CameraController.MainCamera.WorldToScreenPoint(targetpoint);
            return screenPos;
        }
    }
    Vector3 upperPoint => transform.position + Vector3.up * OccupiedBy.Hexes.Count
            * ConfigsManager.Instance.GamePropertiesConfig.DefaultHexThickness;

    protected override void Awake()
    {
        base.Awake();
        bomb = ObjectPool.Instance.GetFromPool(ObjectPool.PoolObjectType.BombSign, targetPos);
    }

    private void Update()
    {
        if (!exploded)
        {           
            bomb.transform.position = Vector3.Lerp(bomb.transform.position, targetPos, Time.deltaTime * 10f);
        }
      
    }
    
    public override void OnStackPopped()
    {
        if (!exploded)
        {
            ObjectPool.Instance.GetFromPool(ObjectPool.PoolObjectType.Explosion);
            foreach (var item in GameManager.Instance.CurrentLevel.HexGrid.GetNeighbors(this))
            {
                if (item.IsOccupied) item.OccupiedBy.TryToPop(true);
            }
            exploded = true;
            bomb.ReturnToPool();
            bomb = null;    
        }         
    }
    private void OnDestroy()
    {
        if(bomb) bomb.ReturnToPool();
    }
}
