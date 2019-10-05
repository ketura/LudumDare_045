using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using Utilities;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource mainMusicSource;
    AudioSource secondaryMusicSource;

    public AudioMixerGroup musicGroup;
    public AudioMixerGroup sfxGroup;

    public void PlayMusic(AudioClip clip)
    {
        mainMusicSource.Pause();

        var childObject = new GameObject();
        secondaryMusicSource = childObject.AddComponent<AudioSource>();
        secondaryMusicSource.clip = clip;
        secondaryMusicSource.loop = true;
        secondaryMusicSource.volume = 1.0f;
        secondaryMusicSource.outputAudioMixerGroup = musicGroup;
        secondaryMusicSource.Play();
        childObject.transform.parent = transform;
    }

    public void ResumeMainMusic()
    {
        if (secondaryMusicSource)
        {
            Destroy(secondaryMusicSource.gameObject);
            mainMusicSource.Play();
        }
    }

	public AudioSource PlayClip(AudioClip clip, float volume = 1.0f)
	{
		return PlayClip(clip, volume, 1, false);
	}

    public AudioSource PlayClipLooping(AudioClip clip, float volume = 1.0f)
    {
        return PlayClip(clip, volume, 1, true);
    }

    public AudioSource PlayClip(AudioClip clip, float volume, int times, bool looping)
    {
        var childObject = new GameObject();
        var audioSource = childObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.loop = looping;
        audioSource.volume = volume;
        audioSource.outputAudioMixerGroup = sfxGroup;
        audioSource.Play();
        childObject.transform.parent = transform;

        if(!looping)
            Destroy(childObject, clip.length);

        return audioSource;
    }
}
