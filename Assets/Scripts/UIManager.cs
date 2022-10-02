using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public AudioSource MainSceneAudio;

    public void ClickedPauseBtn() {
        GameObject.Find("UI/Canvas/Menu").SetActive(true);
        Time.timeScale = 0.0f;
        MainSceneAudio.Pause();
    }

    public void ClickedResumeBtn()
    {
        GameObject.Find("UI/Canvas/Menu").SetActive(false);
        Time.timeScale = 1.0f;
        MainSceneAudio.Play();
    }

    public void SetSlider(float value)
    {
        MainSceneAudio.outputAudioMixerGroup.audioMixer.SetFloat("MainVolume", value);
    }
}
