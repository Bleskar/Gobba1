using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            PlayMusic(startMusic);

            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //initialize sound effects
        for (int i = 0; i < soundLibrary.Length; i++)
        {
            AudioSource s = gameObject.AddComponent<AudioSource>();
            soundLibrary[i].source = s;

            s.volume = soundLibrary[i].volume;
            s.clip = soundLibrary[i].clip;
        }

        //initialize music
        music = gameObject.AddComponent<AudioSource>();
        music.Play();
        music.loop = true;

        PlayMusicLocal(startMusic);
    }

    [Range(0f, 1f)] public float masterVolume = 1f;

    [Header("Sound Effects")]
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [SerializeField] Sound[] soundLibrary = new Sound[0];

    [Header("Music")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [SerializeField] float transitionTime = .5f;
    [SerializeField] Sound[] musicLibrary = new Sound[0];
    Sound currentMusic;
    AudioSource music;
    [SerializeField] string startMusic;

    private void Update()
    {
        music.pitch = Mathf.Lerp(music.pitch, PlayerCombat.Instance.Dead ? 0f : 1f, Time.unscaledDeltaTime * 2f);
    }

    public static void Play(string name)
        => Instance.PlayLocal(name);

    void PlayLocal(string name)
        => Array.Find(soundLibrary, i => i.name == name).Play();

    public static void PlayMusic(string name)
        => Instance.PlayMusicLocal(name);

    void PlayMusicLocal(string name)
    {
        if (currentMusic != null && currentMusic.name == name)
            return;

        StartCoroutine(Transition(name));
    }

    IEnumerator Transition(string name)
    {
        float timer = transitionTime / 2f;

        if (currentMusic != null)
        {
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                music.volume = currentMusic.volume * musicVolume * masterVolume * 2f * (timer / transitionTime);
                yield return null;
            }
        }

        music.volume = 0f;
        currentMusic = Array.Find(musicLibrary, i => i.name == name);
        music.clip = currentMusic.clip;
        music.Play();

        timer = transitionTime / 2f;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            music.volume = currentMusic.volume * musicVolume * masterVolume * (1f - (2f * (timer / transitionTime)));
            yield return null;
        }

        music.volume = currentMusic.volume * musicVolume * masterVolume;
    }
}

[Serializable]
public class Sound
{
    public string name;

    [HideInInspector] public AudioSource source;
    
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;

    public float pitch = 1f;
    public float pitchRandom = 0f;

    public void Play()
    {
        source.pitch = pitch + UnityEngine.Random.Range(-pitchRandom, pitchRandom);
        source.Play();
    }
}
