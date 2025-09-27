using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Objects")]
    [SerializeField] private AudioSource soundFXPrefab;
    [SerializeField] private int poolSize = 10;
    [Header("Music Clips")]
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioSource musicSource;
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] audioClips;
    private int currentMusicIndex = 0;
    private bool isMusicPlaying = false;


    private List<AudioSource> audioSourcePool;
    private Dictionary<string, AudioClip> clipDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializePool();
        InitializeDictionary();

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = false;
        }
    }

    private void Start()
    {
        PlayMusic();
    }

    private void Update()
    {
        // Check if current music finished and play the next one
        if (isMusicPlaying && !musicSource.isPlaying)
        {
            PlayNextMusic();
        }
    }

    private void InitializeDictionary()
    {
        clipDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClips)
        {
            if (clip != null && !clipDictionary.ContainsKey(clip.name))
                clipDictionary.Add(clip.name, clip);
        }
    }

    // ===== Pool Initialization =====
    private void InitializePool()
    {
        audioSourcePool = new List<AudioSource>();

        for (int i = 0; i < poolSize; i++)
        {
            AudioSource newSource = Instantiate(soundFXPrefab, transform);
            newSource.gameObject.SetActive(false);
            audioSourcePool.Add(newSource);
        }
    }

    private AudioSource GetPooledSource()
    {
        foreach (var source in audioSourcePool)
        {
            if (!source.gameObject.activeInHierarchy)
                return source;
        }

        AudioSource newSource = Instantiate(soundFXPrefab, transform);
        newSource.gameObject.SetActive(false);
        audioSourcePool.Add(newSource);
        return newSource;
    }

    public void PlaySoundFX(string clipName, Vector3 position, float volume = 1f, float minPitch = 1, float maxPitch = 1, bool loop = false)
    {

        if (clipName == null || !clipDictionary.ContainsKey(clipName))
        {
            Debug.LogWarning($"AudioManager: Clip '{clipName}' not found!");
            return;
        }
        AudioClip clip = clipDictionary[clipName];
        AudioSource source = GetPooledSource();
        source.transform.position = position;
        source.volume = volume * GlobalVariables.Instance.masterVolume * GlobalVariables.Instance.SFXVolume;
        // Randomize pitch between minPitch and maxPitch
        source.pitch = Random.Range(minPitch, maxPitch);
        source.gameObject.SetActive(true);
        source.PlayOneShot(clip);

        StartCoroutine(DisableAfter(source, clip.length));
    }

    public int GetClipLength(string clipName)
    {
        if (!clipDictionary.ContainsKey(clipName))
        {
            Debug.LogWarning($"AudioManager: Clip '{clipName}' not found!");
            return 0;
        }
        AudioClip clip = clipDictionary[clipName];
        return Mathf.CeilToInt(clip.length);
    }

    public void PlayRandomSoundFX(string[] clipNames, Vector3 position, float volume = 1f, float minPitch = 1, float maxPitch = 1)
    {
        if (clipNames == null || clipNames.Length == 0) return;

        // Pick a random name
        int index = Random.Range(0, clipNames.Length);
        string clipName = clipNames[index];

        // Play using the existing string-based method
        PlaySoundFX(clipName, position, volume, minPitch, maxPitch);
    }

    private System.Collections.IEnumerator DisableAfter(AudioSource source, float time)
    {
        yield return new WaitForSeconds(time);
        source.gameObject.SetActive(false);
    }

    // ===== MUSIC =====
    public void PlayMusic()
    {
        if (musicClips.Length == 0) return;

        currentMusicIndex = Random.Range(0, musicClips.Length);
        musicSource.clip = musicClips[currentMusicIndex];
        musicSource.volume = GlobalVariables.Instance.masterVolume * GlobalVariables.Instance.musicVolume;
        musicSource.Play();
        isMusicPlaying = true;
    }

    public void PlayNextMusic()
    {
        if (musicClips.Length == 0) return;

        currentMusicIndex = (currentMusicIndex + 1) % musicClips.Length;
        musicSource.clip = musicClips[currentMusicIndex];
        musicSource.volume = GlobalVariables.Instance.masterVolume * GlobalVariables.Instance.musicVolume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        isMusicPlaying = false;
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    public void SetMusicVolume()
    {
        if (musicSource != null)
        {
            musicSource.volume = GlobalVariables.Instance.masterVolume * GlobalVariables.Instance.musicVolume;
        }
    }
}