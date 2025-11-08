using UnityEngine;
using UnityEngine.UI;

public class LevelProgressBar : MonoBehaviour
{
    [SerializeField] Image bar;
    float targetValue;

    void OnEnable()
    {
        Level.OnLevelProgressChanged += UpdateBar;
        GameManager.Instance.OnlevelLoadingSarted += Reset;       
    }
    void OnDisable() => Level.OnLevelProgressChanged -= UpdateBar;

    void UpdateBar(float value)
    {
        targetValue = value / GameManager.Instance.CurrentLevelConfig.TargetPoints;
        bar.fillAmount = Mathf.Lerp(bar.fillAmount, targetValue, Time.deltaTime * 10f);
    }

    private void Reset()
    {
        targetValue = 0f;
        bar.fillAmount = 0f;
    }
}