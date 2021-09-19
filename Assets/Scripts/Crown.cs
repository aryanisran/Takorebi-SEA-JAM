using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crown : MonoBehaviour
{
    public PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Collected());

        }
    }

    public IEnumerator Collected()
    {
        player.GetComponent<Animator>().SetTrigger("Buff");
        player.SwitchForm();
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
}
