using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip debugClip;

    public static SoundManager Instance { get; private set; }

    public enum SoundType
    {
        Music,
        Ambiant,
        SFX
    }

    [Serializable]
    public class SoundOptions
    {
        public SoundType soundType = SoundType.SFX;
        public bool solo = false;
        public float volume = 1f;
        public float pitch = 1f;
        public bool loop = false;
    }

    private List<AudioSource> musicSources = new List<AudioSource>();
    private List<AudioSource> ambiantSources = new List<AudioSource>();
    private List<AudioSource> sfxSources = new List<AudioSource>();

    private Transform musicsHolder;
    private Transform ambiantsHolder;
    private Transform sfxsHolder;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }
#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            var options = new SoundOptions
            {
                soundType = SoundType.SFX,
                solo = false,
                volume = 1f,
                loop = false
            };
            PlaySound(options, debugClip);
        }
    }
#endif

    private void InitializeAudioSources()
    {
        // Create GameObject to hold AudioSources if not already present
        musicsHolder = new GameObject("MusicsHolder").transform;
        musicsHolder.parent = this.transform;
        GameObject musicSource = new GameObject("MusicSource");
        musicSource.transform.parent = musicsHolder;
        musicSources.Add(musicSource.AddComponent<AudioSource>());
        musicSources[0].loop = true;

        ambiantsHolder = new GameObject("AmbiantsHolder").transform;
        ambiantsHolder.parent = this.transform;
        GameObject ambiantSource = new GameObject("AmbiantSource");
        ambiantSource.transform.parent = ambiantsHolder;
        ambiantSources.Add(ambiantSource.AddComponent<AudioSource>());

        sfxsHolder = new GameObject("SFXsHolder").transform;
        sfxsHolder.parent = this.transform;
        GameObject sfxSource = new GameObject("SFXSource");
        sfxSource.transform.parent = sfxsHolder;
        sfxSources.Add(sfxSource.AddComponent<AudioSource>());
    }

    public void PlaySound(SoundOptions options, AudioClip clip)
    {
        List<AudioSource> targetSources = new List<AudioSource>();
        Transform targetHolder = null;
        switch (options.soundType)
        {
            case SoundType.Music:
                targetSources = musicSources;
                targetHolder = musicsHolder;
                break;
            case SoundType.Ambiant:
                targetSources = ambiantSources;
                targetHolder = ambiantsHolder;
                break;
            case SoundType.SFX:
                targetSources = sfxSources;
                targetHolder = sfxsHolder;
                break;
        }
        if (options.solo)
        {
            foreach (var source in targetSources)
            {
                source.Stop();
                source.gameObject.SetActive(false);
            }
        }
        AudioSource availableSource = targetSources.Find(source => !source.isPlaying);
        if (availableSource == null)
        {
            GameObject newSourceObj = new GameObject(options.soundType.ToString() + "Source");
            availableSource = newSourceObj.AddComponent<AudioSource>();
            targetSources.Add(availableSource);
            newSourceObj.transform.parent = this.transform;
        }
        availableSource.volume = options.volume;
        availableSource.pitch = options.pitch;
        availableSource.loop = options.loop;
        availableSource.clip = clip;
        availableSource.gameObject.SetActive(true);
        availableSource.transform.parent = targetHolder;
        availableSource.Play();
    }

    public void StopSound(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Music:
                foreach (var source in musicSources)
                {
                    source.Stop();
                }
                break;
            case SoundType.Ambiant:
                foreach (var source in ambiantSources)
                {
                    source.Stop();
                }
                break;
            case SoundType.SFX:
                foreach (var source in sfxSources)
                {
                    source.Stop();
                }
                break;
        }
    }
}
