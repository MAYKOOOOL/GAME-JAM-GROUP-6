using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        AudioManager.Instance.PlaySFX("Click");
        SceneManager.LoadSceneAsync(1);
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayBGM("BGM 2");
    }

    public void Click()
    {
        AudioManager.Instance.PlaySFX("Click");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
