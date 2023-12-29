using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject optionsMenu;
    public GameObject mainMenu;


    public void Options()
    {
        SoundManager.Instance.PlaySFX("PressButton");

        if (optionsMenu.activeSelf)
            {
                mainMenu.SetActive(true);
                optionsMenu.SetActive(false);

            }
            else
            {
                mainMenu.SetActive(false);
                optionsMenu.SetActive(true);
            }
    }

    public void Quit()
    {
        SoundManager.Instance.PlaySFX("PressButton");
        Application.Quit();
    }

    public void Play()
    {
        SoundManager.Instance.PlaySFX("PressButton");
        StartCoroutine(waitForASecondInPlay());
    }

    IEnumerator waitForASecondInPlay()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(1);
    }
}
