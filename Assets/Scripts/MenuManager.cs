using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject menuPanel;

    private void Awake()
    {
        Cursor.visible = true;
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
}
