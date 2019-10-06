using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public float Power = 0.5f;
    public float NozzleRotationSpeed = 360f;
    public float SpeedIncrease=0.1f;
    public GameObject NozzleRotationBase;
    public ParticleSystem exaustParticleSystem;

    private Quaternion targetNozzleRotation;
    private ParticleSystem.MainModule exaustParticleSystemMainModule;

    // Start is called before the first frame update
    void Start()
    {
        exaustParticleSystemMainModule = exaustParticleSystem.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (NozzleRotationBase.transform.rotation != targetNozzleRotation)
        {
            NozzleRotationBase.transform.rotation = Quaternion.RotateTowards(NozzleRotationBase.transform.rotation, targetNozzleRotation, NozzleRotationSpeed * Time.deltaTime);
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
            exaustParticleSystemMainModule.startSpeed = exaustSpeed * 2;
        }
        else
        {
            TurnParticlesOff();
        }
    }

    public void TurnParticlesOn()
    {
        if (!exaustParticleSystem.isPlaying)
            exaustParticleSystem.Play();
    }

    public void TurnParticlesOff()
    {
        if (!exaustParticleSystem.isStopped)
            exaustParticleSystem.Stop();
    }
}
