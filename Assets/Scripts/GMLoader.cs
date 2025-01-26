using UnityEngine;

public class GMLoader : MonoBehaviour
{
    public GameManager theGM;

    void Awake()
    {
        if (FindObjectOfType<GameManager>() == null)
        {
            GameManager.instance = Instantiate(theGM);
            DontDestroyOnLoad(AudioManager.instance.gameObject);
        }
    }
}
