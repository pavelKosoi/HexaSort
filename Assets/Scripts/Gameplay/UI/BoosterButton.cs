using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoosterButton : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI countTxt;
    BoosterConfig boosterConfig;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }


    public void Init(BoosterConfig config)
    {
        boosterConfig = config;
        SetLock(boosterConfig == null);        
    }

    void SetLock(bool locked)
    {
        if (!locked)
        {
            icon.sprite = boosterConfig.Icon;
            (icon.transform as RectTransform).anchoredPosition = new Vector2(0, 50);
            (icon.transform as RectTransform).sizeDelta = boosterConfig.IconSize;
            countTxt.gameObject.SetActive(true);
            UpdateCounter();
        }
        else
        {
            icon.sprite = ConfigsManager.Instance.GamePropertiesConfig.LockIcon;
            (icon.transform as RectTransform).anchoredPosition = new Vector2(0, 8);
            (icon.transform as RectTransform).sizeDelta = new Vector2(100, 100);
            countTxt.gameObject.SetActive(false);
        }
        button.interactable = !locked;
    }

    public void UpdateCounter()
    {
        if(boosterConfig == null) return;
        int currentAmount = PlayerPrefs.GetInt(boosterConfig.Type.ToString());
        countTxt.text = currentAmount.ToString();
        if (currentAmount == 0) SetLock(true);
    }

    private void OnClick()
    {       
        GameManager.Instance.BoosterManager.ActivateBooster(boosterConfig);
    }
}
