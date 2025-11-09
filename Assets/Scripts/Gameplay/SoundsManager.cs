using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundsManager : MonoBehaviour
{
    #region Fields
    AudioSource audioSource;
    #endregion

    #region Getters
    SoundsConfig soundsConfig => ConfigsManager.Instance.SoundsConfig;
    #endregion

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundOneShot(SoundType soundType) => 
        audioSource.PlayOneShot(soundsConfig.GetSoundByType(soundType).clip);
}
