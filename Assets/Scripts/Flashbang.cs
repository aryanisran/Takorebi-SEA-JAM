using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashbang : MonoBehaviour
{
    public float delayTime;
    List<Guard> guards = new List<Guard>();
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Bang", delayTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Bang()
    {
        foreach(Guard g in guards)
        {
            g.CheckFlashed(transform.position);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Guard")
        {
            guards.Add(other.GetComponent<Guard>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Guard")
        {
            guards.Remove(other.GetComponent<Guard>());
        }
    }
}
