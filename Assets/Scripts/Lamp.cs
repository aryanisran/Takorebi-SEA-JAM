using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    PlayerController player;
    Rigidbody rb;
    [SerializeField] GameObject lamp;

    float fallSpeed = 10f;
    bool inside;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        rb = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == player)
        {
            //player.Die();
            StartCoroutine(Fall());
        }
    }

    public IEnumerator Fall()
    {
        rb.velocity = new Vector3(0, -fallSpeed, 0);
        yield return new WaitForSeconds(1f);
        lamp.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

}
