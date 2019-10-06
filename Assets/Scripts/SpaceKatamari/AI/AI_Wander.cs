using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Wander : EnemyAI
{
    public float directionChangeInterval = 4f;
    public float acceleration = 2f;
    public float maxSpeed = 2f;

    private float directionChangeTimer = 0f;
    public Quaternion dir;

    new void Start()
    {
        base.Start();
        dir = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
    }

    public override void Behave()
    {
        directionChangeTimer += Time.deltaTime;
        if (directionChangeTimer >= directionChangeInterval)
        {
            directionChangeTimer = 0;
            dir = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        }

        Vector3 newVelocity = rigidBody.velocity + (dir * Vector3.forward * acceleration * Time.deltaTime);
        rigidBody.velocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);

        base.Behave();
    }
}
