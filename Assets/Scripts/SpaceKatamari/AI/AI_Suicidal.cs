using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Suicidal : EnemyAI
{
    public float Acceleration = 1f;
    public float MaxSpeed = 3f;
    public float RotationSpeed = 120f;

    private float speed = 0;

    public override void Behave()
    {
        Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        speed = Mathf.Clamp(speed + (Acceleration * Time.deltaTime), 0, MaxSpeed);
        rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity + (targetRotation * new Vector3(0, 0, Acceleration * Time.deltaTime)), MaxSpeed);
    }
}
