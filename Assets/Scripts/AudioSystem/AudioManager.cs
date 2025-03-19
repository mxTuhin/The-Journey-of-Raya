using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [Header("AudioSources")]
    public AudioSource bgmSource;
    public GameObject sfxSourcePrefab;
    private AudioSource sfxSource;
    public AudioSource sfxSourceFlameThrowerOnly;

    [Header("AudioClip")]
    public AudioClip BGMFight;
    public AudioClip BGMAmbient;
    public AudioClip BGMMenu;
    public AudioClip BGMBoss;
    [Range(0f, 1f)] public float bgmVolume = 0.7f;
    [Space]
    [SerializeField] private SoundAudioClipPair[] soundAudioClipPairs;

    [Header("Footsteps")]
    public List<AudioClip> footstepClips;


    private float DefaultAudioVolumeMin = 0.7f;
    private float DefaultAudioVolumeMax = 0.7f;

    private float fadeDuration = 0.75f;


    //Bools
    private bool hearAudibleSFX;
    private bool hearAudibleBGM;

    private static AudioManager Instance;


    private static readonly Dictionary<Sound, SoundAudioClipPair> SoundToClip = new Dictionary<Sound, SoundAudioClipPair>();
    
    private ObjectPool<AudioSource> audioSourcePool;
    [SerializeField] private int defaultPoolSize = 10;
    
    private AudioSource audioSource;
    private AudioClip currentBGM;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        PreferenceData preferenceData = GameManager.GetInstance().GetPreferenceData();

        bool isBgmOn = preferenceData.isBgmOn;
        bool isSfxOn = preferenceData.isSfxOn;

        SetBGMPermissionValue(isBgmOn);
        SetSFXPermissionValue(isSfxOn);

        Init();
        PlayBGMMenu();
    }

    private void OnLevelWasLoaded(int level)
    {
        // Call BGM Off for the game scenes
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            bgmSource.Stop();
            return;
        }

        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            PlayBGMMenu();
        }

        else
        {
            PlayBGMAmbient();
        }
    }

    #region AudioSourcePoolInitialization

    private void InitAudioSourcePool()
    {
        // Initialize the pool
        audioSourcePool = new ObjectPool<AudioSource>(
            CreateAudioSource,
            OnGetAudioSource,
            OnReleaseAudioSource,
            OnDestroyAudioSource,
            true,  // Collection check
            defaultPoolSize,
            defaultPoolSize * 2
        );
    }

    private AudioSource CreateAudioSource()
    {
        AudioSource source = Instantiate(sfxSourcePrefab, transform).GetComponent<AudioSource>();
        source.gameObject.SetActive(false);
        return source;
    }

    private void OnGetAudioSource(AudioSource source)
    {
        source.gameObject.SetActive(true);
    }

    private void OnReleaseAudioSource(AudioSource source)
    {
        source.Stop();
        source.gameObject.SetActive(false);
    }

    private void OnDestroyAudioSource(AudioSource source)
    {
        Destroy(source.gameObject);
    }    

    #endregion

    public void Init()
    {
        InitAudioSourcePool();
        
        foreach (var pair in soundAudioClipPairs)
        {
            if (SoundToClip.ContainsKey(pair.sound))
            {
                SoundToClip[pair.sound] = pair;
                continue;
            }

            SoundToClip.Add(pair.sound, pair);
        }
    }

    public void OnWorldStart()
    {
        // PlayBGMAmbient();
    }


    public static AudioManager CallPlaySFX(Sound sound)
    {
        Instance.PlaySFX(sound);
        return Instance;
    }

    public AudioManager PlaySFX(Sound sound)
    {
        TriggerSFX(sound);
        return this;
    }

    public AudioManager SetSound(float volumeMin, float volumeMax)
    {
        if (sfxSource == null)
            return this;
        sfxSource.volume = GetSFXVolume(volumeMin, volumeMax);
        return this;
    }

    private void TriggerSFX(Sound sound)
    {
        if (!hearAudibleSFX)
            return;
        if (SoundToClip.ContainsKey(sound))
        {
            try
            {
                sfxSource = audioSourcePool.Get();
            }
            catch
            {
                // CLog.Print("Error Set");
            }

            try
            {
                // sfxSource.volume = 0;
                // sfxSource.Stop();
                sfxSource.gameObject.SetActive(true);
                sfxSource.volume = GetSFXVolume(SoundToClip[sound].volumeRange, SoundToClip[sound].volumeRange);
                sfxSource.PlayOneShot(SoundToClip[sound].audioClip);
                StartCoroutine(HideSFXSourceAfterPlayback(SoundToClip[sound].audioClip, sfxSource));
            }
            catch
            {
                // CLog.Print("Source not playing");
            }
        }
    }

    private void TriggerSFX(AudioClip clip, float volumeMin, float volumeMax)
    {
        if (!hearAudibleSFX)
            return;

        if (clip != null)
        {
            try
            {
                sfxSource = audioSourcePool.Get();
            }
            catch
            {
                // CLog.Print("Error Set");
            }

            try
            {
                // sfxSource.volume = 0;
                // sfxSource.Stop();
                sfxSource.gameObject.SetActive(true);
                sfxSource.volume = GetSFXVolume(volumeMin, volumeMax);
                sfxSource.PlayOneShot(clip);
                StartCoroutine(HideSFXSourceAfterPlayback(clip, sfxSource));
            }
            catch
            {
                // CLog.Print("Source not playing");
            }
        }
    }

    IEnumerator HideSFXSourceAfterPlayback(AudioClip clip, AudioSource sourceSFX)
    {
        yield return new WaitForSeconds(clip.length);
        audioSourcePool.Release(sourceSFX);
    }

    public void PlayBGM()
    {
        if (!hearAudibleBGM)
            return;

        ChangeBGM(currentBGM, GetBGMVolume());
    }

    public void PlayBGMAmbient()
    {
        if (!hearAudibleBGM)
            return;

        ChangeBGM(BGMAmbient, 0.6f);
    }

    public void PlayBGMFight()
    {
        if (!hearAudibleBGM)
            return;

        ChangeBGM(BGMFight, GetBGMVolume());
    }

    public void PlayBGMMenu()
    {
        if (!hearAudibleBGM)
            return;

        ChangeBGM(BGMMenu, GetBGMVolume());

    }

    public void PlayBGMBoss()
    {
        if (!hearAudibleBGM)
            return;

        ChangeBGM(BGMBoss, 0.5f);
    }

    public void ChangeBGM(AudioClip newBGM, float volume)
    {
        StartCoroutine(FadeMusic(bgmSource, newBGM, fadeDuration, volume));
    }

    private IEnumerator FadeMusic(AudioSource audioSource, AudioClip newClip, float duration, float targetVolume)
    {
        float startVolume = audioSource.volume;
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / duration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();

        currentBGM = newClip;
        audioSource.clip = currentBGM;

        currentTime = 0;
        audioSource.volume = 0;
        audioSource.Play();

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(0, targetVolume, currentTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    #region Internal Methods

    private float GetSFXVolume(float _value1, float _value2)
    {
        // if (!hearAudibleSFX)
        //     return 0.0f;
        float level;
        level = Random.Range(_value1, _value2);
        // level = 0;
        return level;
    }

    private float GetBGMVolume()
    {
        // if (!hearAudibleBGM)
        //     return 0.0f;
        return bgmVolume;
    }

    public bool IsSfxPlayable()
    {
        return hearAudibleSFX;
    }

    #endregion

    #region Setters

    public void SetSFXPermission(bool canPlaySFX)
    {
        SetSFXPermissionValue(canPlaySFX);
        if (!canPlaySFX)
        {
            sfxSource.Stop();
            sfxSource.volume = 0.0f;
        }
        TriggerSFX(Sound.ButtonClick);
    }

    public void SetSFXPermissionValue(bool canPlaySFX)
    {
        hearAudibleSFX = canPlaySFX;
        //TO DO: Save Can Play Audio SFX
    }

    public void SetBGMPermission(bool canPlayBGM)
    {
        SetBGMPermissionValue(canPlayBGM);
        TriggerSFX(Sound.ButtonClick);
    }

    public void SetBGMPermissionValue(bool canPlayBGM)
    {
        hearAudibleBGM = canPlayBGM;
        // TO DO: Save Can Play Audio BGM

        if (!canPlayBGM)
        {
            bgmSource.Stop();
            bgmSource.volume = 0.0f;
        }
        else
        {
            PlayBGM();
        }
    }



    #endregion
    
    public static AudioManager GetInstance()
    {
        return Instance;
    }


    [Serializable]
    private class SoundAudioClipPair
    {
        public Sound sound;
        public AudioClip audioClip;
        [Range(0, 1)]
        public float volumeRange = 0.7f;
    }




    #region Gameplay Audio Control
    public void PlayShootSFX(Sound sound)
    {
        // sfxSource.pitch = Random.Range(0.8f, 1.2f);
        PlaySFX(sound);
    }

    public void PlayPoisonedSfx(Sound sound)
    {
        if (!hearAudibleSFX) return;
        AudioSource audioSource = sfxSourcePrefab.TryGetComponent(out AudioSource s) ? s : null;
        audioSource.loop = true;
        audioSource.clip = SoundToClip[sound].audioClip;
        audioSource.volume = GetSFXVolume(SoundToClip[sound].volumeRange, SoundToClip[sound].volumeRange);
        audioSource.Play();
    }

    public void StopPoisenedSfx()
    {
        if (!hearAudibleSFX) return;

        AudioSource audioSource = sfxSourcePrefab.TryGetComponent(out AudioSource s) ? s : null;

        if (audioSource.clip == null) return;
        if (audioSource.clip != SoundToClip[Sound.ToxicDamage].audioClip) return;

        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = null;
    }

    public void PlayFootstep()
    {
        if (!hearAudibleSFX) return;

        TriggerSFX(footstepClips[Random.Range(0, footstepClips.Count)], 0.15f, 0.3f);
    }

    private bool canPlayCollectSfx = true;

    public void PlayCollectionSfx(Sound sound)
    {
        if (!canPlayCollectSfx) return;

        PlaySFX(sound);
        canPlayCollectSfx = false;

        StartCoroutine(CollectSfxCoolDown());
    }

    private IEnumerator CollectSfxCoolDown()
    {
        yield return new WaitForSeconds(0.075f);
        canPlayCollectSfx = true;
    }

    public void PlayFrenzySfx()
    {
        if (!hearAudibleSFX) return;

        StartCoroutine(PlayFrenzy());
    }

    private IEnumerator PlayFrenzy()
    {
        if (!hearAudibleSFX) yield break;

        bgmSource.volume = 0.1f;

        CallPlaySFX(Sound.Frenzy);

        yield return new WaitForSeconds(2.5f);

        bgmSource.volume = GetBGMVolume();
    }




    #endregion
}
