using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float range = 50f;
    public float cooldown = 1f;
    public float acquisitionInterval = 1f;
    public float rotationSpeed = 90f;
    public GameObject TurretHead;
    public GameObject bulletType;
    public GameObject bulletSpawnPoint;
    public AudioSource audioSource;

    private float cooldownTimer = 0f;
    private float targetAcquisitionTimer = 0f;

    public Ship currentTarget = null;
    public Ship myShip;

    // Start is called before the first frame update
    void Start()
    {
        UpdateShip();
    }

    // Update is called once per frame
    void Update()
    {
		if (myShip == null)
			return;

		if(currentTarget != null && (currentTarget.currentTeam == myShip.currentTeam || currentTarget.currentTeam == Ship.Team.Neutral))
		{
			currentTarget = null;
		}

        targetAcquisitionTimer += Time.deltaTime;
        if (targetAcquisitionTimer >= acquisitionInterval)
        {
            targetAcquisitionTimer = 0f;
            if (currentTarget == null || Vector3.Distance(transform.position, currentTarget.transform.position) > range)
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
            currentTarget = closestShip;
        }
    }

    private bool AimWeapon()
    {
        Quaternion targetRotation = Quaternion.LookRotation(currentTarget.transform.position - transform.root.position);
        TurretHead.transform.rotation = Quaternion.RotateTowards(TurretHead.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        return TurretHead.transform.rotation == targetRotation;
    }

    private void FireWeapon()
    {
        GameObject bullet = GameObject.Instantiate(bulletType, bulletSpawnPoint.transform.position, TurretHead.transform.rotation);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.myTeam = myShip.currentTeam;
        }

        if (audioSource != null)
            audioSource.Play();        
    }
}
