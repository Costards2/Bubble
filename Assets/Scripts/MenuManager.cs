using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject menuPanel;

    public Slider _musicSlider, _sfxSlider;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        _musicSlider.value = 1;//AudioManager.instance.audioSourceMusic.volume;
        _sfxSlider.value = 1;//AudioManager.instance.audioSourceSFX.volume;
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void Back()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void Sair()
    {
        Debug.Log("Sair");
        Application.Quit();
    }

    public void MusicVolume(float volume)
    {
        AudioManager.instance.MusicVolume(_musicSlider.value);
    }

    public void SFXVolume(float volume)
    {
        AudioManager.instance.SFXVolume(_sfxSlider.value);
    }
}
