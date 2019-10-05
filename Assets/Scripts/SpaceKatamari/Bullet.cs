using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f;
    public Ship.Team myTeam = Ship.Team.Neutral;

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        Ship ship = collision.transform.root.GetComponent<Ship>();
        Matter matter = collision.gameObject.GetComponent<Matter>();
        if (matter != null && (ship == null || ship.currentTeam == Ship.Team.Neutral || ship.currentTeam != myTeam))
        {
            //do damage
            int i = 1;
        }

        Destroy(this.gameObject);
    }
}
