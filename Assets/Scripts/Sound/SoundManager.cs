using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum SoundType
{
    BGM,
    EFFECT,
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioMixer audioMixer;
    private float currentBGMVolume, currentEffectVolume;
    private Dictionary<string, AudioClip> clipsDic;
    [SerializeField] private AudioClip[] preLoadClips;
    private List<TemporaySoundPlayer> instantiatedSounds;

    private void Awake()
    {
        clipsDic = new Dictionary<string, AudioClip>();

        foreach (AudioClip clip in preLoadClips)
        {
            clipsDic.Add(clip.name, clip);
        }
        instantiatedSounds = new List<TemporaySoundPlayer>();

        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

        Managers.Game.GameOverAction -= () => PlaySound2D("SFX GameOver");
        Managers.Game.GameOverAction += () => PlaySound2D("SFX GameOver");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        instantiatedSounds.Clear();
        InitVoumes(-2, -2);
        if(scene.name == "Game")
        {
            Instance.PlaySound2D("FLUX STAGE", 0, true, SoundType.BGM);
        }
        else if(scene.name == "Lobby")
        {
            Instance.PlaySound2D("FLUX MAIN", 0, true, SoundType.BGM);
        }
        else if(scene.name == "Title")
        {
            Instance.PlaySound2D("FLUX TITLE", 0, true, SoundType.BGM);
        }
    }

    private AudioClip GetClip(string clipName)
    {
        AudioClip clip = clipsDic[clipName];

        if (clip == null)
        {
            Debug.LogError(clipName + "is not find");
            return null;
        }

        return clip;
    }

    private void AddToList(TemporaySoundPlayer soundPlayer)
    {
        instantiatedSounds.Add(soundPlayer);
    }

    public void StopLoopSound(string clipName)
    {
        foreach(TemporaySoundPlayer audioPlayer in instantiatedSounds)
        {
            if(audioPlayer.ClipName == clipName)
            {
                instantiatedSounds.Remove(audioPlayer);
                Destroy(audioPlayer.gameObject);
                return;
            }
        }
        Debug.LogError(clipName + "is not find (StopLoopSound)");
    }

    public void PlaySound2D(string clipName, float delay = 0f, bool isLoop = false, SoundType type = SoundType.EFFECT)
    {
        GameObject soundObj = new GameObject("TemporarySoundPlayer 2D");
        TemporaySoundPlayer soundPlayer = soundObj.AddComponent<TemporaySoundPlayer>();

        if(isLoop)
        {
            AddToList(soundPlayer);
        }
        soundPlayer.InitSound2D(GetClip(clipName));
        soundPlayer.Play(audioMixer.FindMatchingGroups(type.ToString())[0], delay, isLoop);
    }

    public void InitVoumes(float bgm, float effect)
    {
        SetVolumes(SoundType.BGM, bgm);
        SetVolumes(SoundType.EFFECT, effect);
    }

    public void SetVolumes(SoundType type, float value)
    {
        audioMixer.SetFloat(type.ToString(), value);
    }
}
