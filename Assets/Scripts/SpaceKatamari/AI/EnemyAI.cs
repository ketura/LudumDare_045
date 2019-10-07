using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float acquisitionInterval = 1f;
    public float acquisitionRange = 100;

    private float acquisitionTimer = 0f;
    protected GameObject player = null;
    protected Rigidbody rigidBody;
    protected Ship myShip;
    protected Thruster[] thrusters;

    // Start is called before the first frame update
    protected void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        myShip = GetComponent<Ship>();
        thrusters = GetComponentsInChildren<Thruster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myShip != null && myShip.Active)
        {
            acquisitionTimer += Time.deltaTime;
            if (acquisitionTimer >= acquisitionInterval)
            {
                acquisitionTimer = 0f;
                player = null;
                PlayerKatamari[] katamaris = FindObjectsOfType<PlayerKatamari>();
                if (katamaris.Length > 0)
                {
                    PlayerKatamari katamari = katamaris[0];
                    if (katamari.CurrentState == PlayerState.Existing  && Vector3.Distance(transform.position, katamari.transform.position) <= acquisitionRange)
                    {
                        player = katamari.gameObject;
                    }
                }
            }

            if (player != null)
            {
                Behave();
                if (thrusters.Length > 0)
                {
                    if (rigidBody.velocity.magnitude > 0)
                    {
                        Quaternion thrusterRotation = Quaternion.LookRotation(rigidBody.velocity.normalized, Vector3.up);
                        foreach (Thruster thruster in thrusters)
                        {
                            thruster.SetExaustSpeed(rigidBody.velocity.magnitude);
                            thruster.SetNozzleRotation(thrusterRotation);
                        }
                    }
                    else
                    {
                        foreach (Thruster thruster in thrusters)
                        {
                            thruster.TurnParticlesOff();
                        }
                    }
                }
            }
            else
            {
                rigidBody.velocity = Vector3.zero;
            }
        }
    }

    public virtual void Behave()
    {
        transform.Rotate(new Vector3(0, 10 * Time.deltaTime, 0));
    }
}
