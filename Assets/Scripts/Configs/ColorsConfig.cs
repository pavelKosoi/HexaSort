using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorsConfig", menuName = "Configs/ColorsConfig")]
public class ColorsConfig : ScriptableObject
{
    #region fields
    [SerializeField] Color[] colors;
    #endregion

    #region Getters
    public Color[] Colors => colors;
    #endregion

    public Color[] GetRandomColors(int amount)
    {
        amount = Mathf.Clamp(amount, 0, colors.Length);
        var toRet = colors.OrderBy(c => Random.value).Take(amount).ToArray();
        return toRet;
    }
}
