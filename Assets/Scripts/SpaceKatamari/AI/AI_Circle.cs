using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Circle : EnemyAI
{
    public float speed = 2f;
    public float rotationSpeed = 15f;

    private float dir;
    private int rotationDirection;
    
    new void Start()
    {
        base.Start();

        dir = Random.Range(0f, 360f);

        int coinflip = Random.Range(0, 1);
        if (coinflip == 0)
            rotationDirection = 1;
        else
            rotationDirection = -1;
    }

    public override void Behave()
    {
        dir += rotationSpeed * rotationDirection * Time.deltaTime;
        if (dir > 360)
            dir -= 360;
        else if (dir < 0)
            dir += 360;

        rigidBody.velocity = Quaternion.Euler(0, dir, 0) * (Vector3.forward * speed);

        base.Behave();
    }
}
