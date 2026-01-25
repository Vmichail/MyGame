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
    private Transform playerTransform;
    private List<AudioSource> activeSFX = new();
    private List<AudioSource> audioSourcePool;
    private Dictionary<string, AudioClip> clipDictionary;

    private readonly List<string> mainMenuSounds = new()
    {
        "uiDeny",
        "UpgradeBoughtSound",
        "buttonHighlight1Sound",
        "levelup2",
        "UnlockSound",
    };

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

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerTransform = playerObj.transform;
        else
            Debug.LogWarning("AudioManager: Player object with tag 'Player' not found in the scene.");
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

    public void PlayRandomSoundFX(string[] clipNames, Vector2 position, float volume = 1f, float minPitch = 1, float maxPitch = 1, bool applyDistance = false)
    {
        if (clipNames == null || clipNames.Length == 0) return;

        // Pick a random name
        int index = Random.Range(0, clipNames.Length);
        string clipName = clipNames[index];

        // Play using the existing string-based method
        PlaySoundFX(clipName, position, volume, minPitch, maxPitch, applyDistance: applyDistance);
    }

    public AudioSource PlaySoundFX(string clipName, Vector2 position, float volume = 1f, float minPitch = 1, float maxPitch = 1, bool loop = false, bool applyDistance = false)
    {
        if (GlobalVariables.Instance.mainMenuScene && !mainMenuSounds.Contains(clipName))
            return null;

        if (string.IsNullOrEmpty(clipName) || !clipDictionary.ContainsKey(clipName))
        {
            Debug.LogWarning($"AudioManager: Clip '{clipName}' not found!");
            return null;
        }

        AudioClip clip = clipDictionary[clipName];
        AudioSource source = GetPooledSource();
        source.transform.position = position;
        //volume based on distance
        float distance = Vector2.Distance(position, playerTransform.position);
        float minDistance = 1f;
        float maxDistance = 70f;
        float distanceFactor = Mathf.Clamp01(1 - (distance - minDistance) / (maxDistance - minDistance));
        if (applyDistance)
        {
            //Debug.Log($"AudioManager: Playing '{clipName}' at distance {distance} with volume factor {distanceFactor} and total volume:{volume * distanceFactor * GlobalVariables.Instance.masterVolume * GlobalVariables.Instance.SFXVolume}");
            source.volume = volume * distanceFactor * GlobalVariables.Instance.masterVolume * GlobalVariables.Instance.SFXVolume;
        }
        else
        {
            source.volume = volume * GlobalVariables.Instance.masterVolume * GlobalVariables.Instance.SFXVolume;
        }
        source.pitch = Random.Range(minPitch, maxPitch);
        source.loop = loop;
        source.gameObject.SetActive(true);
        if (!activeSFX.Contains(source))
            activeSFX.Add(source);

        if (loop)
        {
            source.clip = clip;
            source.Play();
        }
        else
        {
            source.clip = null;
            source.PlayOneShot(clip);
            StartCoroutine(DisableAfter(source, clip.length));
        }

        return source;
    }

    public void StopSound(AudioSource source)
    {
        if (source == null) return;

        if (activeSFX.Contains(source))
            activeSFX.Remove(source);

        source.Stop();
        source.loop = false;
        source.clip = null;
        source.gameObject.SetActive(false);
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

    private System.Collections.IEnumerator DisableAfter(AudioSource source, float time)
    {
        yield return new WaitForSeconds(time);
        if (activeSFX.Contains(source))
            activeSFX.Remove(source);
        source.gameObject.SetActive(false);
    }

    // ===== MUSIC =====
    public void PlayMusic(int index = -1)
    {
        if (musicClips.Length == 0) return;

        if (index < 0)
            currentMusicIndex = Random.Range(0, musicClips.Length - 1);
        else
            currentMusicIndex = index;

        musicSource.clip = musicClips[currentMusicIndex];
        musicSource.volume = GlobalVariables.Instance.masterVolume * GlobalVariables.Instance.musicVolume;
        musicSource.Play();
        isMusicPlaying = true;
    }

    public void PlayMusic(string musicName)
    {
        if (string.IsNullOrEmpty(musicName))
            return;

        for (int i = 0; i < musicClips.Length; i++)
        {
            AudioClip clip = musicClips[i];

            if (clip != null && clip.name == musicName)
            {
                PlayMusic(i);   // reuse your existing logic
                return;
            }
        }

        Debug.LogWarning($"AudioManager: Music '{musicName}' not found in musicClips!");
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

    public void PauseAllSFX()
    {
        foreach (var s in activeSFX)
            if (s != null) s.Pause();
    }

    public void ResumeAllSFX()
    {
        foreach (var s in activeSFX)
            if (s != null) s.UnPause();
    }
}