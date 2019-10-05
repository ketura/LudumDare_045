using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float range = 50f;
    public float cooldown = 1f;
    public float acquisitionInterval = 1f;
    public float rotationSpeed = 15f;
    public GameObject bulletType;
    public GameObject bulletSpawnPoint;

    private float cooldownTimer = 0f;
    private float targetAcquisitionTimer = 0f;

    public GameObject currentTarget = null;
    private Ship myShip;

    // Start is called before the first frame update
    void Start()
    {
        UpdateShip();
    }

    // Update is called once per frame
    void Update()
    {
        targetAcquisitionTimer += Time.deltaTime;
        if (targetAcquisitionTimer >= acquisitionInterval)
        {
            targetAcquisitionTimer = 0f;
            AcquireTarget();
        }

        bool weaponAimed = false;
        if (currentTarget != null)
        {
            weaponAimed = AimWeapon();
        }

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else if (currentTarget != null && weaponAimed && Vector3.Distance(transform.root.position, currentTarget.transform.position) <= range)
        {
            FireWeapon();
            cooldownTimer = cooldown;
        }
    }

    public void UpdateShip()
    {
        currentTarget = null;
        myShip = transform.root.GetComponent<Ship>();
    }

    private void AcquireTarget()
    {
        Ship[] ships = FindObjectsOfType<Ship>();

        Ship.Team myTeam = myShip.currentTeam;
        Ship closestShip = null;
        float closestDistance = range;

        foreach (Ship ship in ships)
        {
            if (!ship.Active || ship.currentTeam == myTeam || ship.currentTeam == Ship.Team.Neutral)
            {
                continue;
            }
            
            float distance = Vector3.Distance(transform.position, ship.transform.position);
            if (distance <= range && distance < closestDistance)
            {
                closestShip = ship;
                closestDistance = distance;
            }
        }

        if (closestShip == null)
        {
            currentTarget = null;
        }
        else
        {
            currentTarget = closestShip.gameObject;
        }
    }

    private bool AimWeapon()
    {
        Quaternion targetRotation = Quaternion.LookRotation(currentTarget.transform.position - transform.root.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        return transform.rotation == targetRotation;
    }

    private void FireWeapon()
    {
        GameObject bullet = GameObject.Instantiate(bulletType, bulletSpawnPoint.transform.position, transform.rotation);
        bullet.transform.rotation = transform.rotation;
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.myTeam = myShip.currentTeam;
        }
    }
}
