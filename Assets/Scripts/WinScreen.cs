using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public void Menu()
    {
        AudioManager.instance.Play("Collect");
        Time.timeScale = 1;
        AudioManager.instance.Play("Old BGM");
        AudioManager.instance.Stop("Young BGM");
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
