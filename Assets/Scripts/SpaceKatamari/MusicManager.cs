using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Utilities.Singleton<MusicManager>
{
    public AudioSource AudioSource;
    public AudioClip GrowTrack;
    public AudioClip ChillTrack;
    public AudioClip IntenseTrack;

    private AudioClip nextTrack = null;
    private float nextTrackTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextTrackTimer > 0)
        {
            nextTrackTimer -= Time.deltaTime / 2;
            AudioSource.volume = nextTrackTimer;
            if (nextTrackTimer <= 0)
            {
                AudioSource.Stop();
                AudioSource.volume = 1f;

                if (nextTrack != null)
                {
                    AudioSource.clip = nextTrack;
                    AudioSource.Play();
                }
            }
        }
    }

    private void PlayTrack(AudioClip track)
    {
        if (AudioSource.clip == track)
            return;

        if (AudioSource.isPlaying)
        {
            nextTrack = track;
            if (nextTrackTimer <= 0)
            {
                nextTrackTimer = 1f;
            }
        }
        else if (track != null)
        {
            AudioSource.clip = track;
            AudioSource.Play();
        }
    }

    public void StopMusic()
    {
        PlayTrack(null);
    }

    public void PlayGrowthMusic()
    {
        PlayTrack(GrowTrack);
    }

    public void PlayChillMusic()
    {
        PlayTrack(ChillTrack);
    }

    public void PlayIntenseMusic()
    {
        PlayTrack(IntenseTrack);
    }
}
