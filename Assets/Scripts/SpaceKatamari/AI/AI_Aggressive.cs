using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Aggressive : EnemyAI
{
    public float RotationSpeed = 120f;
    public float HoverRange = 10f;

    public float acceleration = 0.5f;
    public float maxSpeed = 2f;
    private float speed = 0f;
    private float shimmySpeed = 0;
    private bool shimmyPositive = true;

    public override void Behave()
    {
        UpdateForwardSpeed();
        UpdateShimmySpeed();       
        Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        rigidBody.velocity = transform.rotation * new Vector3(shimmySpeed, 0, speed);
        
    }

    private void UpdateForwardSpeed()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > HoverRange)
        {
            speed = Mathf.Clamp(speed + (acceleration * Time.deltaTime), -maxSpeed, maxSpeed);
        }
        else
        {
            speed = Mathf.Clamp(speed - (acceleration * Time.deltaTime), -maxSpeed, maxSpeed);
        }
    }

    private void UpdateShimmySpeed()
    {
        if (shimmyPositive)
        {
            shimmySpeed += acceleration * Time.deltaTime;
            if (shimmySpeed >= maxSpeed)
            {
                shimmyPositive = false;
            }
        }
        else
        {
            shimmySpeed -= acceleration * Time.deltaTime;
            if (shimmySpeed <= -maxSpeed)
            {
                shimmyPositive = true;
            }
        }
    }
}
