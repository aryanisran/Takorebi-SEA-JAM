using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        AudioManager.instance.Play("Collect");
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        AudioManager.instance.Play("Collect");
        Application.Quit();
    }
}
