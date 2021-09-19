using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crown : MonoBehaviour
{
    public PlayerController player;
    [SerializeField] GameObject sprite;

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
        sprite.GetComponent<Animator>().SetTrigger("Buff");
        sprite.GetComponent<Animator>().SetBool("Walk", false);
        player.SwitchForm();
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
}
