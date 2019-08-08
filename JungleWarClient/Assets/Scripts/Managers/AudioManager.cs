using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
    #region 路径
    private const string PATH_PREFIX = "Sounds/";
    public const string ALERT = "Alert";
    public const string ARROW_SHOOT = "ArrowShoot";
    public const string BG_FAST = "Bg(fast)";
    public const string BG_MODERATE = "Bg(moderate)";
    public const string BUTTON_CLICK = "ButtonClick";
    public const string MISS = "Miss";
    public const string SHOOT_PERSON = "ShootPerson";
    public const string TIMER = "Timer";
    #endregion

    #region 声音源
    private AudioSource background_audio_source;
    private AudioSource normal_audio_source;
    #endregion

    public AudioManager()
    {
        Init();
    }

    public void Init()
    {
        GameObject audio_source_go = new GameObject("AudioSourceGameObject");
        background_audio_source = audio_source_go.AddComponent<AudioSource>();
        normal_audio_source = audio_source_go.AddComponent<AudioSource>();

        PlayAudioClip(background_audio_source, LoadAudioClip(BG_MODERATE), 0.3f, true);
    }

    public void PlayBackgroundAudioClip(string clip_name)
    {
        PlayAudioClip(background_audio_source, LoadAudioClip(clip_name), 0.5f, true);
    }
    public void PlayNormalAudioClip(string clip_name)
    {
        PlayAudioClip(normal_audio_source, LoadAudioClip(clip_name), 1, false);
    }

    private void PlayAudioClip(AudioSource source, AudioClip clip, float volume, bool loop = false)
    {
        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
        source.Play();
    }

    private AudioClip LoadAudioClip(string sound_path)
    {
        return Resources.Load<AudioClip>(PATH_PREFIX + sound_path);
    }
}
