using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashbang : MonoBehaviour
{
    public float delayTime;
    List<Guard> guards = new List<Guard>();
    public ParticleSystem explode, smoke;
    // Start is called before the first frame update
    void Start()
    {
        smoke.Play();
        Invoke("PlayParticles", delayTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayParticles()
    {
        smoke.Stop();
        explode.Play();
        Invoke("Bang", 0.5f);
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
