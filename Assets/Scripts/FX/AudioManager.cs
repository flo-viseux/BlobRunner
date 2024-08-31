using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager
{
    public enum GroupType
    {
        Master,
        Ambient,
        SFX,
        UI,
    }
    
    private AudioMixer _mixer;
    private string _parameter;
    

    public AudioManager(AudioMixer mixer)
    {
        _mixer = mixer;
    }

    private string GetMixerParam(GroupType type)
    {
        switch (type)
        {
            case GroupType.Master : return "Master_Volume";
            case GroupType.Ambient : return "Ambiant_Volume";
            case GroupType.SFX : return "SFX_Volume";
            case GroupType.UI : return "UI_SFX_Volume";
            default: return "";
        }
    }
    
    public void ChangeVolume(GroupType paramType, float targetVolume, float duration)
    {
        _parameter = GetMixerParam(paramType);
        _mixer.GetFloat(_parameter, out float currentVolume);
        DOTween.To(GetCurrentVolume, SetCurrentVolume, targetVolume, duration);
    }

    private float GetCurrentVolume()
    {
        _mixer.GetFloat(_parameter, out float currentVolume);
        return currentVolume;
    }

    private void SetCurrentVolume(float volume)
    {
        _mixer.SetFloat(_parameter, volume);
    }

    public void SetParamVolume(GroupType paramType, float volume)
    {
        _mixer.SetFloat(GetMixerParam(paramType), volume);
    }

    public bool IsParamPlaying(GroupType paramType)
    {
       _mixer.GetFloat(GetMixerParam(paramType), out float currentVolume);
       return currentVolume > -70f;
    }
}
