using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f;
    public Ship.Team myTeam = Ship.Team.Neutral;
    public float speed = 10f;

    private float lifetimeTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifetimeTimer += Time.deltaTime;
        if (lifetimeTimer >= lifetime)
        {
            Destroy(this.gameObject);
        }

        Vector3 velocity = new Vector3(0, 0, speed * Time.deltaTime);
        transform.position = transform.position + (transform.rotation * velocity);
    }

    void OnTriggerEnter(Collider other)
    {
        Ship ship = other.transform.root.GetComponent<Ship>();
        Matter matter = other.gameObject.GetComponent<Matter>();
        if (matter != null)
        {
            if (ship == null || ship.currentTeam == Ship.Team.Neutral || ship.currentTeam != myTeam)
            {
                // do damage
                Destroy(this.gameObject);
            }
        }
    }
}
