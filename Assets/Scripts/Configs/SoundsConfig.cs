using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundsConfig", menuName = "Configs/SoundsConfig")]
public class SoundsConfig : ScriptableObject
{

    #region Classes&Structures
    [Serializable]
    public class Sound
    {
        public SoundType type;
        public AudioClip clip;
    }
    #endregion

    #region Fields

    [SerializeField] Sound[] sounds;
    Dictionary<SoundType, Sound> cached = new Dictionary<SoundType, Sound>();
    #endregion

    #region Getters
    public Sound[] Sounds => sounds;
    #endregion

    public Sound GetSoundByType(SoundType type)
    {
        if(cached.ContainsKey(type)) return cached[type];
        else
        {
            var sound = sounds.FirstOrDefault(s => s.type == type);
            cached[type] = sound;
            return sound;
        }
    }
}
public enum SoundType
{
    None,
    Pop
}
