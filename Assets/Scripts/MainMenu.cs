using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void ClickedStartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ClickedQuitBtn()
    {
        Application.Quit();
    }

    public void EnableUI()
    {
        GameObject.Find("Canvas/frontPanel/UGUI").SetActive(true);
    }
}
