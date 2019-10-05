using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
   private float size=0;
    public float maxSize;
    public float regen;
    public float damageAmount;
    public GameObject emmiter;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (emmiter.GetComponent<Matter>().Attached && size < maxSize) {
            size += regen;
        }
        transform.localScale = new Vector3(size, size, size);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Bullet") {
            Destroy(other.gameObject);
            size-= damageAmount;

        }
    }
}
