using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
       
        SceneManager.LoadSceneAsync(2);
        //AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayBGM("BGM2");
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
