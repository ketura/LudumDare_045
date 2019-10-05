using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private float size = 0;
    public float maxSize;
    public float regen;
    public float damageAmount;
    public GameObject emitter;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (emitter.GetComponent<Matter>().Attached && size < maxSize)
        {
            size += regen;
        }
        transform.localScale = new Vector3(size, size, size);
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, size);
        foreach (Collider c in hitColliders)
        {
            OnBulletHit(c);
        }


    }



    private void OnBulletHit(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject);
            size -= damageAmount;

        }
    }
}
