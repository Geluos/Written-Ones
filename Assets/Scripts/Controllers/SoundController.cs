using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MusicType { None, Adventure, Act1, Act2, Act3, Titles }

public class SoundController : Controller<SoundController>
{
    private static readonly System.Random random = new();
    private const float FADE_SECONDS = 5;
    private float musicVolume = 0.5f;
    private float effectsVolume = 0.5f;
    public AudioSource musicSource;
    public AudioSource effectsSource;

    public List<AudioClip> AdventureSceneMusic;
    public List<AudioClip> Act1Music;
    public List<AudioClip> Act2Music;
    public List<AudioClip> Act3Music;
    public AudioClip CardStartPlay;
    public AudioClip AxeSound;
    public AudioClip KnifeSound;
    public AudioClip FluteSound;
    public AudioClip MonsterSound;

	public AudioClip Titles;
	private MusicType musicType;

    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            musicSource.volume = value;
        }
    }

    public float EffectsVolume
    {
        get => effectsVolume;
        set
        {
            effectsVolume = value;
            effectsSource.volume = value;
        }
    }

    public void Start()
    {
        musicType = MusicType.Adventure;
        musicSource.volume = MusicVolume;
        effectsSource.volume = EffectsVolume;
    }

    private void Update()
    {
        if (musicType == MusicType.None)
            return;

        if (musicSource.isPlaying)
            return;

        AudioClip music = musicType switch
        {
            MusicType.Adventure => RandomListItem(AdventureSceneMusic),
            MusicType.Act1 => RandomListItem(Act1Music),
            MusicType.Act2 => RandomListItem(Act2Music),
            MusicType.Act3 => RandomListItem(Act3Music),
			MusicType.Titles => Titles,
			_ => null
        };

        musicSource.clip = music;
        musicSource.Play();
        StartCoroutine(FadeIn());
    }

    public void PlayAdventureMusic()
    {
        musicType = MusicType.Adventure;
        musicSource.Stop();
    }


    public void PlaySound(AudioClip sound)
    {
        effectsSource.PlayOneShot(sound);
    }

    private static T RandomListItem<T>(List<T> lst)
    {
        int idx = random.Next(0, lst.Count);
        return lst[idx];
    }

    IEnumerator FadeIn()
    {
        musicSource.volume = 0;
        float timeElapsed = 0;

        while (musicSource.volume < MusicVolume)
        {
            musicSource.volume = Mathf.Lerp(0, MusicVolume, timeElapsed / FADE_SECONDS);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
