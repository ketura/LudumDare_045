using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public float Power = 0.5f;
    public float NozzleRotationSpeed = 360f;
    public float SpeedIncrease=0.1f;
    public GameObject NozzleRotationBase;
    public ParticleSystem exhaustParticleSystem;
    public AudioSource audioSource;

    private Quaternion targetNozzleRotation;
    private ParticleSystem.MainModule exhaustParticleSystemMainModule;

    // Start is called before the first frame update
    void Start()
    {
        exhaustParticleSystemMainModule = exhaustParticleSystem.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (NozzleRotationBase.transform.rotation != targetNozzleRotation)
        {
            NozzleRotationBase.transform.rotation = Quaternion.RotateTowards(NozzleRotationBase.transform.rotation, targetNozzleRotation, NozzleRotationSpeed * Time.deltaTime);
        }

        if (audioSource != null)
        {
            if (exhaustParticleSystem.isPlaying && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
            else if (!exhaustParticleSystem.isPlaying && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    public void SetNozzleRotation(Quaternion rotation)
    {
        targetNozzleRotation = rotation;
    }

	public void SetExaustSpeed(float exaustSpeed)
	{
		if (exaustSpeed > 0)
		{
			TurnParticlesOn();
			if(exhaustParticleSystem != null)
			{
				exhaustParticleSystemMainModule.startSpeed = exaustSpeed * 2;
			}
           
		}
		else
		{
			TurnParticlesOff();
		}
	}

    public void TurnParticlesOn()
    {
        if (exhaustParticleSystem != null && !exhaustParticleSystem.isPlaying)
            exhaustParticleSystem.Play();
    }

    public void TurnParticlesOff()
    {
        if (exhaustParticleSystem != null && !exhaustParticleSystem.isStopped)
            exhaustParticleSystem.Stop();
    }
}
